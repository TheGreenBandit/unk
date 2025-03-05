using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float> //we can change this to be seperate between crouch / sprint etc 
    {
        public static float Value = .5f;

        public override void Update()
        {
            if (!Enabled) return;
            PlayerController.instance.MoveSpeed = Value / 5;
        }

        public override void OnDisable()
        {
            PlayerController.instance.MoveSpeed = .5f;
        }
    }
}
