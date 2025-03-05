using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class LightsOn : ToggleCheat
    { //this isnt workig, should get rid of it
        public override void Update()
        {
            if (!Enabled)
                return;
            LightManager.instance.Reflect().SetValue("debugMode", true);
        }

        public override void OnDisable()
        {
            LightManager.instance.Reflect().SetValue("debugMode", false);
        }
    }
}
