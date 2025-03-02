using HarmonyLib;
using Photon.Pun;
using unk.Cheats.Core;
using unk.Manager;
using unk.Menu.Core;
using unk.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace unk
{
    public class unk : MonoBehaviour
    {
        private List<ToggleCheat> cheats;
        private Harmony harmony;
        private unkMenu menu;

        private static unk instance;
        public static unk Instance
        {
            get
            {
                if (instance == null) instance = new unk();
                return instance;
            }
        }

        public void Start()
        {
            instance = this;
            ThemeUtil.LoadTheme("Default");
            LoadCheats();
            DoPatching();
            LoadKeybinds();
            this.StartCoroutine(GameObjectManager.Instance.CollectObjects());
        }

        private void DoPatching()
        {
            harmony = new Harmony("unk");
            Harmony.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void LoadCheats()
        {
            cheats = new List<ToggleCheat>();
            menu = new unkMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "unk.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(ToggleCheat))))
            {
                cheats.Add((ToggleCheat)Activator.CreateInstance(type));
            }
        }

        private void LoadKeybinds()
        {
            
        }

        public void FixedUpdate()
        {
            try
            {
                if(PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.FixedUpdate());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in FixedUpdate: {e}");
            }
        }

        public void Update()
        {
            try
            {
                if (Input.GetKeyDown(Settings.MenuToggleKey))
                {
                    Settings.b_isMenuOpen = !Settings.b_isMenuOpen;

                    if(Settings.b_isMenuOpen)
                    {
                        MenuUtil.ShowCursor();
                    }
                    else
                    {
                        MenuUtil.HideCursor();
                    }
                }

                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.Update());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in Update: {e}");
            }
        }

        public void OnGUI()
        {
            try
            {
                if (Event.current.type == EventType.Repaint)
                {
                    VisualUtil.DrawString(new Vector2(5f, 2f), "unk", new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);

                    if (MenuUtil.resizing)
                    {
                        string rTitle = $"Resizing Menu\nLeft Click to Confirm, Right Click to Cancel\n{unkMenu.Instance.windowRect.width}x{unkMenu.Instance.windowRect.height}";


                        VisualUtil.DrawString(new Vector2(Screen.width / 2, 35f), rTitle, Settings.c_espPlayers, true, true, true, 22);
                        MenuUtil.ResizeMenu();
                    }

                    if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.OnGui());
                }

                menu.Draw();
            }
            catch (Exception e)
            {
                Debug.Log($"Error in OnGUI: {e}");
            }
        }
    }
}
