using UnityEngine;
using Unk.Cheats.Core;
using Unk.Menu.Tab;

namespace Unk.Cheats
{
    internal class OverrideAnimSpeed : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 1.0f;
        public override void Update()
        {
            if (PlayersTab.selectedPlayer is null || !Enabled) return;

            PlayersTab.selectedPlayer.photonView.RPC("OverrideAnimationSpeedActivateRPC", Photon.Pun.RpcTarget.All,
                true, Value, .1f, .1f, .3f * Time.fixedDeltaTime);
        }
    }
}