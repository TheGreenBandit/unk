using unk.Manager;
using unk.Util;
using System.Linq;
using UnityEngine;

namespace unk
{
    public static class Extensions
    {
        public static Camera GetCamera(this MainCamera mainCamera)
        {
            return mainCamera.Reflect().GetValue<Camera>("cam");
        }

        public static Vector3 GetClosestMonster(this Vector3 point) => GameObjectManager.monsters.OrderBy(x => Vector3.Distance(x.transform.position, point)).FirstOrDefault().transform.position;
    }
}
