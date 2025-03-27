using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class ServerTab : MenuTab
    {
        public ServerTab() : base("Server") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header("Server", true);

                UI.Button("Voice Chat All", () => PlayerAvatar.instance.Reflect().GetValue<PlayerVoiceChat>("playerVoice").ToggleLobby(true));
                UI.Button("Voice Chat Proximity", () => PlayerAvatar.instance.Reflect().GetValue<PlayerVoiceChat>("playerVoice").ToggleLobby(false));
            });
        }
    }
}
