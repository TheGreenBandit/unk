using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float> //we can change this to be seperate between crouch / sprint etc 
    {
        public float Value { get; set; } = .5f;

        public override void Update()
        {
            PlayerController.instance.MoveSpeed = Value;
        }

        public override void OnDisable()
        {
            PlayerController.instance.MoveSpeed = .5f;
        }
    }
}
