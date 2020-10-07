using Fair.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Fair.Resources
{
    public class HungerPlayer : ResourcePlayer
    {
        public bool IsMoving { get => player.velocity.X > Vector2.Zero.X && player.velocity.Y > Vector2.Zero.Y; }
        public int UseCounter;

        public bool IsSated { get => player.HasBuff(BuffID.WellFed); }
        public const float SatedMax = 24f;
        public float SatedAmount;

        public bool IsStarving { get => HungerCurrent == 0; }
        public int StarvingBadLifeRegenCounter { get => (StarvingCounter + MaxStarvingCounter); }
        public const int StarvingCounterMax = 25;
        public const int MaxStarvingCounterMax = 12;
        public int StarvingCounter;
        public int MaxStarvingCounter;
        public List<int> StarvingSymptomBuffs;

        public const int HungerMax = byte.MaxValue;
        public int HungerCurrent;

        public Dictionary<Fullness, Action> DecrementMap;

        public Fullness GetFullness()
        {
            if (HungerCurrent == HungerMax) return Fullness.Full;
            if (HungerCurrent >= 200) return Fullness.GoodOnFood;
            if (HungerCurrent >= 175) return Fullness.ALittleHungry;
            if (HungerCurrent >= 150) return Fullness.OK;
            if (HungerCurrent >= 125) return Fullness.GettingHungry;
            if (HungerCurrent >= 100) return Fullness.Peckish;
            if (HungerCurrent >= 1) return Fullness.LowOnFood;
            return Fullness.Starving;
        }

        public void Decrement(byte amount)
        {
            if (IsStarving) return;

            Fullness oldFull = GetFullness();
            if (IsMoving) HungerCurrent--;
            HungerCurrent -= (amount - ((IsSated ? 1 : 0) + (SatedAmount > 8f ? 1 : 0)));
            ClampHunger();

            if (!IsSated) SatedAmount -= 1;
            if (oldFull < GetFullness()) SatedAmount -= 2;
            ClampSated();
        }

        public void CheckStarving()
        {
            if (!IsStarving) return;

            if (IsSated)
            {
                player.ClearBuff(BuffID.WellFed);
                SatedAmount += 15f;
                ClampSated();
            }

            bool sFlag = false;
            SatedAmount -= 0.5f;
            ClampSated();
            if (SatedAmount == 0) sFlag = true;

            if (sFlag)
            {
                StarvingCounter++;
                ClampStarvingCounter();
            }

            StarvingSymptomBuffs.Clear();
            StarvingSymptomBuffs.Add(BuffID.Stinky);
            StarvingSymptomBuffs.Add(BuffID.PotionSickness);
            if (StarvingCounter >= 5 && StarvingCounter < 10) StarvingSymptomBuffs.Add(BuffID.Darkness);
            if (StarvingCounter >= 10 && StarvingCounter < 15) StarvingSymptomBuffs.Add(BuffID.Poisoned);
            if (StarvingCounter >= 15 && StarvingCounter < 25) StarvingSymptomBuffs.Add(BuffID.Slow);
            if (StarvingCounter >= 25) StarvingSymptomBuffs.Add(BuffID.OgreSpit);
            if (StarvingCounter >= 25) StarvingSymptomBuffs.Add(BuffID.Cursed);
            if (StarvingCounter >= 20) StarvingSymptomBuffs.Add(BuffID.Confused);
            if (StarvingCounter >= 20) StarvingSymptomBuffs.Add(BuffID.BrokenArmor);
            if (StarvingCounter >= 15) StarvingSymptomBuffs.Add(BuffID.Venom);
            if (StarvingCounter >= 10) StarvingSymptomBuffs.Add(BuffID.Blackout);

            if (StarvingCounter == StarvingCounterMax)
            {
                MaxStarvingCounter++;
                MaxStarvingCounter = (int)MathHelper.Clamp(MaxStarvingCounter, 0, MaxStarvingCounterMax);
            }

            if (MaxStarvingCounter == MaxStarvingCounterMax)
            {
                player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} Starved."), (int)((player.statLifeMax2 + player.statDefense) * 10f), 0);
            }
        }

        public void ClampHunger() => HungerCurrent = (int)MathHelper.Clamp(HungerCurrent, 0f, HungerMax);
        public void ClampSated() => SatedAmount = MathHelper.Clamp(SatedAmount, 0f, SatedMax);
        public void ClampStarvingCounter() => StarvingCounter = (int)MathHelper.Clamp(StarvingCounter, 0f, StarvingCounterMax);

        public override void Initialize()
        {
            HungerCurrent = HungerMax;

            StarvingSymptomBuffs = new List<int>();
            DecrementMap = new Dictionary<Fullness, Action>() {
                { Fullness.Starving, () => Decrement(0) },
                { Fullness.LowOnFood, () => Decrement(1) },
                { Fullness.Peckish, () => Decrement(1) },
                { Fullness.GettingHungry, () => Decrement(1) },
                { Fullness.OK, () => Decrement(1) },
                { Fullness.ALittleHungry, () => Decrement(1) },
                { Fullness.GoodOnFood, () => Decrement(2) },
                { Fullness.Full, () => Decrement(2) }
            };
        }

        public override void UpdateBadLifeRegen()
        {
            if (IsStarving)
            {
                player.lifeRegen = 0;
                player.lifeRegen -= StarvingBadLifeRegenCounter;
            }
        }

        public override void PreUpdateMovement()
        {
            //Main.NewText(string.Concat(new string[] { "Hunger: ", HungerCurrent.ToString(), "/", HungerMax.ToString(), " ", GetFullness().ToString(), " || ", "Satiness: ", SatedAmount.ToString(), " ", IsSated.ToString(), " || ", "Starving: ", StarvingCounter.ToString(), " ", IsStarving.ToString(), " ", StarvingBadLifeRegenCounter.ToString(), " ", MaxStarvingCounter.ToString()}));
        }

        public override void PreUpdateBuffs()
        {
            foreach (int i in StarvingSymptomBuffs)
            {
                player.AddBuff(i, 1);
            }
        }

        public override void PreUpdate()
        {
            player.AddBuff(ModContent.BuffType<HungerBuff>(), 60);

            if (GetFullness() == Fullness.Peckish)
            {
                player.runAcceleration = 0.111f;
                player.moveSpeed -= 0.2f;
            }
            if (GetFullness() <= Fullness.LowOnFood)
            {
                player.runAcceleration = 0.08f;
                player.moveSpeed -= 0.35f;
            }

            if (GetFullness() < Fullness.ALittleHungry)
            {
                player.allDamage -= 0.05f;
            }
            if (GetFullness() < Fullness.OK)
            {
                player.allDamage -= 0.075f;
            }
            if (GetFullness() < Fullness.GettingHungry)
            {
                player.allDamage -= 0.08f;
            }

            if (UseCounter >= 7)
            {
                HungerCurrent--;
                UseCounter = 0;
                ClampHunger();
            }

            base.PreUpdate();
        }

        public override void Tick(ulong howManyTicks)
        {
            if (howManyTicks % 180 == 0)
            {
                DecrementMap[GetFullness()].Invoke();
            }

            if (howManyTicks % 60 == 0)
            {
                CheckStarving();
            }
        }

        public override void UpdateDead()
        {
            HungerCurrent = HungerMax;
            StarvingCounter = 0;
            MaxStarvingCounter = 0;
            SatedAmount = 0f;
            UseCounter = 0;
        }

        public override TagCompound Save()
        {
            return new TagCompound() {
                { "Hunger", HungerCurrent },
                { "Starving", StarvingCounter },
                { "StarvingMax", MaxStarvingCounter },
                { "SatedAmount", SatedAmount }
            };
        }

        public override void Load(TagCompound tag)
        {
            HungerCurrent = tag.GetInt("Hunger");
            StarvingCounter = tag.GetInt("Starving");
            MaxStarvingCounter = tag.GetInt("StarvingMax");
            SatedAmount = tag.GetFloat("SatedAmount");
        }
    }

    public enum Fullness : byte
    {
        Starving,
        LowOnFood,
        Peckish,
        GettingHungry,
        OK,
        ALittleHungry,
        GoodOnFood,
        Full
    }
}
