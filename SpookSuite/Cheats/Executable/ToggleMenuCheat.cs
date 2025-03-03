using Photon.Pun;
using Unk.Cheats.Core;
using Unk.Util;
using UnityEngine;
using Unk;

namespace SpookSuite.Cheats
{
    internal class ToggleMenuCheat : ExecutableCheat
    {
        public ToggleMenuCheat() : base(KeyCode.Insert) { }

        public override void Execute() 
        {
            Settings.b_isMenuOpen = !Settings.b_isMenuOpen;

            if (!PhotonNetwork.InRoom) return;

            //if (Settings.b_isMenuOpen)
            //    .ShowCursor();
            //else
            //    MenuUtil.HideCursor();
        }

    }
}
