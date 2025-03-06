using System;
using System.Collections.Generic;
using System.Text;
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
                true, Value, 0f, 0f, .3f);
        }
    }
}
