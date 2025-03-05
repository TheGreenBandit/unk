using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class LightsOn : ToggleCheat
    {
        public override void Update()
        {
            LightManager.instance.Reflect().SetValue("debugMode", true);
        }

        public override void OnDisable()
        {
            LightManager.instance.Reflect().SetValue("debugMode", false);
        }
    }
}
