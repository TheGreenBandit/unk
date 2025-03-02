using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        public float Value { get; set; } = 60f;
        private float ogvalue;

        public override void Update()
        {
            Player localp = Network.ഡഢദസഝലബമഡ.GetLocalPlayerSpot().player;
            localp.SetPlayerSpeed(Value);
        }

    }
}
