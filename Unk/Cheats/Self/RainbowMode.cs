using System.Collections;
using UnityEngine;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class RainbowMode : ToggleCheat
    {
        public override void OnEnable()
        {
            Unk.Instance.StartCoroutine(RainbowSuit());
        }

        private IEnumerator RainbowSuit()
        {
            int colors = AssetManager.instance.playerColors.Count;
            int index = 0;
            while (Enabled)
            {
                if (PlayerAvatar.instance.GetLocalPlayer() != null) PlayerAvatar.instance.GetLocalPlayer().PlayerAvatarSetColor(index);
                index = (index + 1) % colors;
                yield return new WaitForSeconds(0.1f); 
            }
        }
    }
}
