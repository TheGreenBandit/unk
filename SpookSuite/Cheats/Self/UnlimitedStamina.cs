using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class UnlimitedStamina : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerController.instance.EnergyCurrent = 100; //done lmao
        }
    }
}
