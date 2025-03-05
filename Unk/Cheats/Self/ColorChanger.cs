using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class ColorChanger : ExecutableCheat, IVariableCheat<int>
    {
        public static int Value = 0;
        public override void Execute()
        {
            PlayerAvatar.instance.PlayerAvatarSetColor(Value);
        }
    }
}
