using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class Invisibility : ToggleCheat
    {
        public override void Update()
        {
            PlayerAvatar.instance.playerAvatarVisuals.isMenuAvatar = true;
        }

        public override void OnEnable()
        {
            PlayerAvatar.instance.playerAvatarVisuals.isMenuAvatar = false;
        }
    }
}