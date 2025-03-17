using Photon.Pun;
using System.Linq;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>//categories (visual, movement, etc)
            {
                UI.Header("Self");
                UI.HorizontalSpace(null, () =>
                {
                    UI.VerticalSpace(() =>
                    {
                        UI.Header("General");
                        //GUILayout.Box(() => {
                        UI.Checkbox("Godmode", Cheat.Instance<Godmode>());
                        UI.Checkbox("Unlimited Energy", Cheat.Instance<UnlimitedStamina>());
                        UI.Checkbox("No Tumble", Cheat.Instance<NoTumble>());
                        UI.Checkbox("Infinite Jump", Cheat.Instance<InfiniteJump>());
                        //});
                    }, GUILayout.Width(100));
                    GUILayout.FlexibleSpace();
                    UI.VerticalSpace(() =>
                    {
                        UI.Header("Network");
                        UI.Checkbox("Use Spoofed Name", Cheat.Instance<NameSpoofer>());
                        UI.Textbox("Spoofed Name", ref NameSpoofer.Value, true, 100);
                    }, GUILayout.Width(100));
                }, GUILayout.Width(UnkMenu.Instance.windowRect.size.x / 2));

                UI.HorizontalSpace(null, () =>
                {
                    UI.Header("Visual");
                    UI.CheckboxV("Invisibility", Cheat.Instance<Invisibility>());
                    UI.CheckboxV("Rainbow Mode", Cheat.Instance<RainbowMode>());
                    UI.TextboxAction("Change Color", ref ColorChanger.Value, 1,
                        new UIButton("Change", Cheat.Instance<ColorChanger>().Execute)
                    );
                }, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                UI.VerticalSpace(() =>
                {
                    UI.Header("Movement");
                    UI.CheatToggleSlider(Cheat.Instance<NoClip>(), "No Clip", NoClip.Value.ToString("#"), ref NoClip.Value, 1f, 20f);
                    UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 1f, 100f);
                }, GUILayout.Width(100));
            }, GUILayout.Width(UnkMenu.Instance.windowRect.size.x / 2));

            UI.Button("Teleport All Items", () =>
            {
                GameObjectManager.items.Where(i => i != null).ToList().ForEach(i =>
                {
                    if (i.GetPhotonTransformView() != null) i.GetPhotonTransformView().Teleport(SemiFunc.MainCamera().transform.position, SemiFunc.MainCamera().transform.rotation);
                });
            });

            if (PhotonNetwork.IsMasterClient) UI.Checkbox("(Host) No Object Money Loss", Cheat.Instance<NoObjectMoneyLoss>());

        }
    }
}