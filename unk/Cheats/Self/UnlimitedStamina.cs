using HarmonyLib;
using unk.Cheats.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace unk.Cheats
{
    internal class UnlimitedStamina : ToggleCheat
    {

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            Player.localPlayer.data.currentStamina = Player.localPlayer.gameObject.GetComponent<PlayerController>().maxStamina;
        }

    }
}
