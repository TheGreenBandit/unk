using HarmonyLib;
using UnityEngine;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    [HarmonyPatch]
    internal class NoObjectMoneyLoss : ToggleCheat
    {
        [HarmonyPatch(typeof(PhysGrabObjectImpactDetector), "OnCollisionStay"), HarmonyPrefix]
        public static bool OnCollisionStay(PhysGrabObjectImpactDetector __instance, Collision collision)
        {
            if (Instance<NoObjectMoneyLoss>().Enabled) return false;
            return true;
        }
    }
}