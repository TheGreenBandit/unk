﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats.Core
{
    public class Cheat : MonoBehaviour
    {
        public bool Enabled { get; set; }
        protected static bool WorldToScreen(Vector3 world, out Vector3 screen)
        {
            //screen = MainCamera.instance.GetCamera().WorldToViewportPoint(world);
            //screen.x *= Screen.width;
            //screen.y *= Screen.height;
            //screen.y = Screen.height - screen.y;
            //return screen.z > 0.0;
            screen = new Vector3(0, 0, 0);
            return true;
        }
        protected float GetDistanceToPlayer(Vector3 position)
        {
            return 0;
            //return (float)Math.Round((double)Vector3.Distance(Player.localPlayer.refs.cameraPos.position, position));
        }
    }
}
