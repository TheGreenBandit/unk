using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class RainbowMode : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            for (int i = 0; i < AssetManager.instance.playerColors.Count; i++) //fixme, wayyy to fast, causes major lag
                PlayerAvatar.instance.PlayerAvatarSetColor(i);
        }
    }
}
