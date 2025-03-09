﻿using UnityEngine;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;
        public override void Update()
        {
            if (!Enabled)
                return;

            CameraZoom.Instance.Reflect().SetValue("zoomPrev", Value);
            CameraZoom.Instance.Reflect().SetValue("zoomNew", Value);
            CameraZoom.Instance.Reflect().SetValue("zoomCurrent", Value);
            CameraZoom.Instance.playerZoomDefault = Value;
            CameraZoom.Instance.SprintZoom = Value;
        }
        public override void OnDisable()
        {
            CameraZoom.Instance.Reflect().SetValue("zoomPrev", 70);
            CameraZoom.Instance.Reflect().SetValue("zoomNew", 70);
            CameraZoom.Instance.Reflect().SetValue("zoomCurrent", 70);
            CameraZoom.Instance.playerZoomDefault = 70;
            CameraZoom.Instance.SprintZoom = 20;
        }
    }
}
