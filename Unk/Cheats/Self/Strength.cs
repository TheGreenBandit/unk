using Unk.Cheats.Core;
using Unk.Handler;
using Unk.Util;

namespace Unk.Cheats
{
    class Strength : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 1;
        public override void Update()
        {
            if (!Enabled) return;
            PlayerAvatar.instance.physGrabber.grabStrength = Value;
            PlayerAvatar.instance.physGrabber.Reflect().GetValue<PhysGrabObject>("grabbedPhysGrabObject")?.OverrideGrabStrength(Value);
        }
        public override void OnDisable()
        {//resets to player stat so no worry about to low strength after upgrading.
            PlayerAvatar.instance.physGrabber.grabStrength = StatsManager.instance.playerUpgradeStrength[PlayerAvatar.instance.GetSteamID()];
        }
    }
}
