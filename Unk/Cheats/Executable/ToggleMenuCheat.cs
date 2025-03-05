using Photon.Pun;
using Unk.Cheats.Core;
using Unk.Util;
using UnityEngine;

namespace Unk.Cheats
{
    internal class ToggleMenuCheat : ExecutableCheat
    {
        public ToggleMenuCheat() : base(KeyCode.Insert) { }

        public override void Execute() 
        {
            Settings.b_isMenuOpen = !Settings.b_isMenuOpen;

            MenuUtil.ToggleCursor();
        }

    }
}
