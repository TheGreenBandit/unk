using Photon.Pun;
using unk.Cheats;
using unk.Cheats.Core;
using unk.Menu.Core;
using System;
using UnityEngine;

namespace unk.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {   
            GUILayout.BeginVertical();
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }
        
        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            if(GUILayout.Button("Suicide"))
                Player.localPlayer.refs.view.RPC("RPCA_PlayerDie", RpcTarget.All, Array.Empty<object>());
            if (GUILayout.Button("Revive"))
                Player.localPlayer.refs.view.RPC("RPCA_PlayerRevive", RpcTarget.All, Array.Empty<object>());
            UI.Checkbox("Godmode", ref Cheat.Instance<Godmode>().Enabled);
            UI.Checkbox("Infinte Jump", ref Cheat.Instance<InfiniteJump>().Enabled);
            UI.Checkbox("No Ragdoll", ref Cheat.Instance<NoRagdoll>().Enabled);
            UI.Checkbox("Unlimited Oxygen", ref Cheat.Instance<UnlimitedOxygen>().Enabled);
            UI.Checkbox("Unlimited Stamina", ref Cheat.Instance<UnlimitedStamina>().Enabled);
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 10f, 100f);
            UI.CheatToggleSlider(Cheat.Instance<SuperJump>(), "Super Jump", SuperJump.Value.ToString("#.#"), ref SuperJump.Value, 0.6f, 20f);

            GUILayout.EndScrollView();
        }
    }
}
