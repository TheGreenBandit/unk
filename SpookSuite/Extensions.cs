using Unk.Util;
using UnityEngine;

namespace Unk
{
    public static class Extensions
    {
        public static Camera GetCamera(this CameraAim mainCamera)
        {
            return mainCamera.GetComponent<Camera>(); //todo
        }
    }
}