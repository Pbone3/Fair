using Terraria.ModLoader;

namespace Fair.Resources
{
    public class ResourcePlayer : ModPlayer
    {
        private ulong _tickCounter;

        public override void Initialize()
        {
            _tickCounter = 0;
        }

        public override void PreUpdate()
        {
            _tickCounter++;
            Tick(_tickCounter);
        }

        public virtual void Tick(ulong howManyTicks)
        {

        }
    }
}
