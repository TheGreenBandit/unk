using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite
{
    public static class Extensions
    {
        public static Camera GetCamera(this FaceCamera mainCamera)
        {
            return mainCamera.GetComponent<Camera>(); //todo make camera thing work?
        }
    }
}
