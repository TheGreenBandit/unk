using System;
using UnityEngine;
using System.Runtime.InteropServices;

using Object = UnityEngine.Object;
using Unk.Menu.Core;
using Unk.Manager;

namespace Unk.Util
{
    internal class MenuUtil
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        public static bool resizing = false;
        public static float MouseX => Input.mousePosition.x;
        public static float MouseY => Screen.height - Input.mousePosition.y;
        public static float maxWidth = Screen.width - (Screen.width * 0.1f);
        public static float maxHeight = Screen.height - (Screen.height * 0.1f);
        private static int oldWidth, oldHeight;

        public static void BeginResizeMenu()
        {
            if (resizing) return;
            WarpCursor();
            resizing = true;
            oldWidth = Settings.i_menuWidth;
            oldHeight = Settings.i_menuHeight;
        }

        public static void WarpCursor()
        {
            float currentX = UnkMenu.Instance.windowRect.x + UnkMenu.Instance.windowRect.width;
            float currentY = UnkMenu.Instance.windowRect.y + UnkMenu.Instance.windowRect.height;

            SetCursorPos((int)currentX, (int)currentY);
        }

        public static void ResizeMenu()
        {
            if (!resizing) return;

            if (Input.GetMouseButtonDown(0))
            {
                resizing = false;
                //Settings.Config.SaveConfig();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                resizing = false;
                Settings.i_menuWidth = oldWidth;
                Settings.i_menuHeight = oldHeight;

               // Settings.Config.SaveConfig();
                return;
            }

            float currentX = UnkMenu.Instance.windowRect.x + UnkMenu.Instance.windowRect.width;
            float currentY = UnkMenu.Instance.windowRect.y + UnkMenu.Instance.windowRect.height;

            Settings.i_menuWidth = (int)Mathf.Clamp(MouseX - UnkMenu.Instance.windowRect.x, 500, maxWidth);
            Settings.i_menuHeight = (int)Mathf.Clamp(MouseY - UnkMenu.Instance.windowRect.y, 250, maxHeight);
            UnkMenu.Instance.Resize();
        }
        public static void ShowCursor()
        {
            Object.FindObjectOfType<CursorManager>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void ToggleCursor()
        {
            if (Cursor.visible)
                HideCursor();
            else
                ShowCursor();
        }
    }
}
