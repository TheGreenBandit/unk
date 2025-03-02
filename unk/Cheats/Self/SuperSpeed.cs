using unk.Cheats.Core;

namespace unk.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 60f;

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = Enabled ? Value : 10f;
        }

    }
}
