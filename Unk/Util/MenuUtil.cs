using UnityEngine;
<<<<<<< Updated upstream
using System.Runtime.InteropServices;
=======
using UnityEngine.InputSystem;
>>>>>>> Stashed changes
using Unk.Menu.Core;

namespace Unk.Util
{
    internal class MenuUtil
    {
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
            float currentY = Screen.height - (UnkMenu.Instance.windowRect.y + UnkMenu.Instance.windowRect.height);
            Mouse.current.WarpCursorPosition(new Vector2(currentX, currentY));
        }

        public static void ResizeMenu()
        {
            if (!resizing) return;

            if (Input.GetMouseButtonDown(0))
            {
                resizing = false;
                Settings.Config.SaveConfig();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                resizing = false;
                Settings.i_menuWidth = oldWidth;
                Settings.i_menuHeight = oldHeight;
                Settings.Config.SaveConfig();
                return;
            }

            float currentX = UnkMenu.Instance.windowRect.x + UnkMenu.Instance.windowRect.width;
            float currentY = UnkMenu.Instance.windowRect.y + UnkMenu.Instance.windowRect.height;

            Settings.i_menuWidth = (int)Mathf.Clamp(MouseX - UnkMenu.Instance.windowRect.x, 500, maxWidth);
            Settings.i_menuHeight = (int)Mathf.Clamp(MouseY - UnkMenu.Instance.windowRect.y, 250, maxHeight);
            UnkMenu.Instance.Resize();
        }

<<<<<<< Updated upstream
=======
        public static void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

>>>>>>> Stashed changes
        public static void HideCursor()
        {
            PlayerController.instance.cameraAim.enabled = true;
            CursorManager.instance.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void ToggleCursor()
        {
<<<<<<< Updated upstream
            showCursor = !showCursor;
            if (!showCursor)
                HideCursor();
=======
            if (Cursor.visible) HideCursor();
            else ShowCursor();
>>>>>>> Stashed changes
        }
    }
}
