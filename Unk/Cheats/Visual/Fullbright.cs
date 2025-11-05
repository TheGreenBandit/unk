using HarmonyLib;
using Mono.Security.Authenticode;
using Photon.Pun;
using System.Reflection;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    [HarmonyPatch]
    class Fullbright : ToggleCheat
    {
        private float og_intensity = 0;
        private Color og_color = Color.white;
        
        public override void OnEnable()
        {
            og_color = RenderSettings.ambientLight;
            og_intensity = RenderSettings.ambientIntensity = 100;
        }

        public override void Update()
        {
            if (Enabled && PhotonNetwork.InRoom)
            {
                FlashlightController.Instance.spotlight.enabled = false;
                RenderSettings.fog = false;
                RenderSettings.ambientLight = Color.white;
                RenderSettings.ambientIntensity = 100;
            }
        }

        public override void OnDisable()
        {
            FlashlightController.Instance.spotlight.enabled = true;
            RenderSettings.fog = true;
            RenderSettings.ambientLight = og_color;
            RenderSettings.ambientIntensity = og_intensity;
        }
    }
}
