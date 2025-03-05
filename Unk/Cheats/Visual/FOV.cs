using UnityEngine;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;
        public override void Update()
        {
            if (!Enabled || GameDirector.instance.MainCamera is null)
                return;
            //fixme sometimes works sometimes not
            GameDirector.instance.MainCamera.fieldOfView = Value;
        }
        public override void OnDisable()
        {
            GameDirector.instance.MainCamera.fieldOfView = 70f;
        }
    }
}
