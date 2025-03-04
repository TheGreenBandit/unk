using UnityEngine;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;

        public override void OnEnable()
        {
            Debug.Log("FOV + " + GameDirector.instance.MainCamera.fieldOfView);
        }
        public override void Update()
        {
            if (PlayerController.instance is null || !Enabled)
                return;

            GameDirector.instance.MainCamera.fieldOfView = Value;
        }
        public override void OnDisable()
        {
            GameDirector.instance.MainCamera.fieldOfView = Value;
        }
    }
}
