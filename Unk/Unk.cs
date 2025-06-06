﻿using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk
{
    public class Unk : MonoBehaviour
    {
        public const string VERSION = "1.4";
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
            ThemeUtil.SetTheme();
            LoadCheats();
            DoPatching();
            AlertUsingUnkMenu();
            GameObjectManager.CollectObjects();
        }

        private void DoPatching()
        {
            try
            {
                harmony = new Harmony("Unk");
                Harmony.DEBUG = true;
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in DoPatching: {e}");
            }
        }

        private void LoadCheats()
        {
            Settings.Changelog.ReadChanges();
            cheats = new List<ToggleCheat>();
            menu = new UnkMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "Unk.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(Cheat))))
            {
                if (type.IsSubclassOf(typeof(ToggleCheat))) cheats.Add((ToggleCheat)Activator.CreateInstance(type));
                else Activator.CreateInstance(type);
                Debug.Log($"Loaded Cheat: {type.Name}");
            }
        }

        public void FixedUpdate()
        {
            try
            {
                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.FixedUpdate());
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
                if (Cheat.instances.Where(c => c.WaitingForKeybind).Count() == 0)
                    Cheat.instances.FindAll(c => c.HasKeybind && Input.GetKeyDown(c.keybind)).ForEach(c =>
                    {
                        if (c.GetType().IsSubclassOf(typeof(ToggleCheat))) ((ToggleCheat)c).Toggle();
                        else if (c.GetType().IsSubclassOf(typeof(ExecutableCheat))) ((ExecutableCheat)c).Execute();
                        else Debug.LogError($"Unknown Cheat Type: {c.GetType().Name}");
                    });

               cheats.ForEach(cheat => cheat.Update());
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
                    VisualUtil.DrawString(new Vector2(5f, 2f), "Unk | " + "Toggle Menu: " + Cheat.Instance<ToggleMenuCheat>().keybind.ToString(), new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);
                cheats.ForEach(cheat => { if (cheat.Enabled) cheat.OnGui(); });
                menu.Draw();
            }
            catch (Exception e)
            {
                Debug.Log($"Error in OnGUI: {e}");
            }
        }
        //playerlistdisplay startloadingrpc
        public void AlertUsingUnkMenu() => PlayerAvatar.instance?.GetLocalPlayer()?.photonView.RPC("ChatMessageSendRPC", RpcTarget.All, "", false);
    }
}
