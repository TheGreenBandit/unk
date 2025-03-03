using HarmonyLib;
using Photon.Pun;
using Unk.Cheats.Core;
using Unk.Menu.Core;
using Unk.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Unk
{
    public class Unk : MonoBehaviour
    {
        private List<ToggleCheat> cheats;
        private Harmony harmony;
        private UnkMenu menu;

        private static Unk instance;
        public static Unk Instance
        {
            get
            {
                if (instance == null) instance = new Unk();
                return instance;
            }
        }

        public void Start()
        {
            instance = this;
            //ThemeUtil.LoadTheme("Default");
            LoadCheats();
            DoPatching();
            LoadKeybinds();
        }

        private void DoPatching()
        {
            harmony = new Harmony("Unk");
            Harmony.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void LoadCheats()
        {
            cheats = new List<ToggleCheat>();
            menu = new UnkMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "Unk.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(ToggleCheat))))
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
                if(Input.GetKeyDown(Settings.MenuToggleKey)) Settings.b_isMenuOpen = !Settings.b_isMenuOpen;

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
                    VisualUtil.DrawString(new Vector2(5f, 2f), "Unk", new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);

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
