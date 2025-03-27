using Mono.Security.Authenticode;
using Photon.Pun;
using UnityEngine;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    class Fullbright : ToggleCheat
    {
        public override void Update()
        {
            if (Enabled)
            {
                FlashlightController.Instance.spotlight.enabled = true;
                FlashlightController.Instance.spotlight.intensity = 100000;
                FlashlightController.Instance.spotlight.range = 100000;
                FlashlightController.Instance.spotlight.transform.localScale = new Vector3(10000, 1000001, 100000);
                FlashlightController.Instance.spotlight.bounceIntensity = 100000;
                FlashlightController.Instance.spotlight.cookieSize = 10000000;
            }
        }
    }
}
