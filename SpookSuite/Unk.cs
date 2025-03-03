using HarmonyLib;
using Photon.Pun;
using Unk.Cheats.Core;
using Unk.Menu.Core;
using Unk.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Unk.Manager;
using Unk.Cheats;

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
            //ThemeUtil.SetTheme("Default");
            LoadCheats();
            DoPatching();
            this.StartCoroutine(GameObjectManager.Instance.CollectObjects());
        }

        private void DoPatching()
        {
            //try
            //{
            //    harmony = new Harmony("SpookSuite");
            //    Harmony.DEBUG = true;
            //    harmony.PatchAll(Assembly.GetExecutingAssembly());
            //}
            //catch (Exception e)
            //{
            //    Debug.Log($"Error in DoPatching: {e}");
            //}
        }

        private void LoadCheats()
        {
            cheats = new List<ToggleCheat>();
            menu = new UnkMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "Unk.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(Cheat))))
            {
                if (type.IsSubclassOf(typeof(ToggleCheat)))
                    cheats.Add((ToggleCheat)Activator.CreateInstance(type));
                else Activator.CreateInstance(type);

                Debug.LogError($"Loaded Cheat: {type.Name}");
            }
        }

        private void LoadKeybinds()
        {
            
        }

        public void FixedUpdate()
        {
            try
            {//for some reason cheat not working??
                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.FixedUpdate());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in FixedUpdate: {e}");
            }
        }

        public void Update() //[Error  : Unity Log] Loaded Cheat: ToggleMenuCheat only this seems to load
        {
            try
            {
                if (Cheat.instances.Where(c => c.WaitingForKeybind).Count() == 0)
                    Cheat.instances.FindAll(c => c.HasKeybind && Input.GetKeyDown(c.keybind)).ForEach(c =>
                    {
                        if (c.GetType().IsSubclassOf(typeof(ToggleCheat))) ((ToggleCheat)c).Toggle();
                        else if (c.GetType().IsSubclassOf(typeof(ExecutableCheat))) ((ExecutableCheat)c).Execute();
                        else Debug.LogError($"Unknown Cheat Type: {c.GetType().Name}");
                    });

                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.Update());
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in Update: {e}");
            }
        }

        public void OnGUI()
        {
            try
            {
                if (Event.current.type == EventType.Repaint)
                    VisualUtil.DrawString(new Vector2(5f, 2f), "SpookSuite| " + "Open / Close: " + Cheat.Instance<ToggleMenuCheat>().keybind.ToString(), new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);
                //cheats.ForEach(cheat => { if (cheat.Enabled) cheat.OnGui(); });
                menu.Draw();
            }
            catch (Exception e)
            {
                Debug.Log($"Error in OnGUI: {e}");
            }
        }

    }
}
