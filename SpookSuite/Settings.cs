﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Unk.Cheats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk
{
    internal class Settings
    {

        /* *    
         * Menu Settings
         * */
        public static bool b_isMenuOpen = false;
        public static int i_menuFontSize = 14;
        public static int i_menuWidth = 810;
        public static int i_menuHeight = 410;
        public static int i_sliderWidth = 100;
        public static int i_textboxWidth = 85;
        public static float f_menuAlpha = 1f;

        /* *    
         * Color Settings
         * */
        public static RGBAColor c_primary = new RGBAColor(165, 55, 253, 1f);
        public static RGBAColor c_menuText = new RGBAColor(255, 255, 255, 1f);
        public static RGBAColor c_espPlayers = new RGBAColor(0, 255, 0, 1f);
        public static RGBAColor c_espItems = new RGBAColor(255, 255, 255, 1f);
        public static RGBAColor c_espEnemies = new RGBAColor(255, 0, 0, 1f);
        public static RGBAColor c_espTraps = new RGBAColor(0, 0, 255, 1f);
        //public static RGBAColor c_chams = new RGBAColor(238, 111, 255, 0.1f);
        public static RGBAColor c_chamItems = new RGBAColor(238, 111, 255, 0.1f);
        public static RGBAColor c_chamMonsters = new RGBAColor(238, 111, 255, 0.1f);
        public static RGBAColor c_chamPlayers = new RGBAColor(238, 111, 255, 0.1f);
        public static RGBAColor c_chamDivingBell = new RGBAColor(238, 111, 255, 0.1f);

        internal class Changelog
        {
            public static List<string> changes;

            public static void ReadChanges()
            {
                changes = new List<string>();

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpookSuite.Resources.Changelog.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        changes.Add(reader.ReadLine());
                    }
                }
            }
        }

        internal class Config
        {
            private static string config = "Unk.config.json";
            private static string defaultConf = "Unk.default.config.json";
            public static void CreateConfigIfNotExists()
            {
                if (HasConfig()) return;

                SaveConfig();
            }

            public static void SaveDefaultConfig()
            {
                SaveConfig(defaultConf);
            }
            public static bool HasConfig()
            {
                return config != null && File.Exists(config);
            }

            public static void SaveConfig()
            {
                SaveConfig(config);
            }

            public static void SaveConfig(string conf)
            {
                Dictionary<string, string> keybinds = new Dictionary<string, string>();
                Dictionary<string, string> toggles = new Dictionary<string, string>();
                Dictionary<string, string> cheatValues = new Dictionary<string, string>();


                Cheat.instances.FindAll(c => !c.Hidden).ForEach(c =>
                {
                    if (c.HasKeybind) keybinds.Add(c.GetType().Name, c.keybind.ToString());
                    if (c is ToggleCheat) toggles.Add(c.GetType().Name, ((ToggleCheat)c).Enabled.ToString());

                    if (c.GetType().GetInterface(typeof(IVariableCheat<>).FullName) != null)
                    {
                        FieldInfo valueField = c.GetType().GetField("Value", BindingFlags.Static | BindingFlags.Public);
                        cheatValues.Add(c.GetType().Name, valueField.GetValue(c).ToString());
                    }
                });


                JObject json = new JObject();
                JObject settings = new JObject();
                JObject colors = new JObject();
                JObject cheatSettings = new JObject();
                JObject reactions = new JObject();

                colors["MenuText"] = JsonConvert.SerializeObject(c_menuText);
                colors["ESPPlayers"] = JsonConvert.SerializeObject(c_espPlayers);
                colors["ESPItems"] = JsonConvert.SerializeObject(c_espItems);
                colors["ESPEnemies"] = JsonConvert.SerializeObject(c_espEnemies);
                colors["ESPTraps"] = JsonConvert.SerializeObject(c_espTraps);
                colors["ChamPlayers"] = JsonConvert.SerializeObject(c_chamPlayers);
                colors["ChamItems"] = JsonConvert.SerializeObject(c_chamItems);
                colors["ChamMonsters"] = JsonConvert.SerializeObject(c_chamMonsters);
                colors["ChamDivingBell"] = JsonConvert.SerializeObject(c_chamDivingBell);

                settings["MenuFontSize"] = i_menuFontSize.ToString();
                settings["MenuWidth"] = i_menuWidth.ToString();
                settings["MenuHeight"] = i_menuHeight.ToString();
                settings["SliderWidth"] = i_sliderWidth.ToString();
                settings["TextboxWidth"] = i_textboxWidth.ToString();
                settings["MenuAlpha"] = f_menuAlpha.ToString();

                json["KeyBinds"] = JObject.FromObject(keybinds);
                json["Toggles"] = JObject.FromObject(toggles);
                json["Values"] = JObject.FromObject(cheatValues);
                json["CheatSettings"] = cheatSettings;
                json["Colors"] = colors;
                json["MenuSettings"] = settings;
                json["Reactions"] = reactions;

                File.WriteAllText(conf, json.ToString());
            }

            public static void LoadConfig()
            {
                CreateConfigIfNotExists();

                string jsonStr = File.ReadAllText(config);
                JObject json = JObject.Parse(jsonStr);

                Debug.Log("Loading Keybinds...");
                if (json.TryGetValue("KeyBinds", out JToken keybindsToken))
                {
                    Cheat.instances.ForEach(c => c.keybind = c.defaultKeybind);
                    foreach (var item in keybindsToken.ToObject<Dictionary<string, string>>())
                    {
                        string s_cheat = item.Key;
                        string s_bind = item.Value;

                        KeyCode bind = Enum.Parse<KeyCode>(s_bind);

                        Cheat.instances.Find(c => c.GetType().Name == s_cheat).keybind = bind;
                    }
                }

                Debug.Log("Loading Toggles...");
                if (json.TryGetValue("Toggles", out JToken togglesToken))
                {
                    foreach (var item in togglesToken.ToObject<Dictionary<string, string>>())
                    {
                        string s_cheat = item.Key;
                        string s_bind = item.Value;

                        bool toggle = bool.TryParse(s_bind, out bool result) ? result : false;

                        if (toggle)
                        {
                            ToggleCheat c = Cheat.instances.Find(c => c.GetType().Name == s_cheat && c is ToggleCheat) as ToggleCheat;
                            if (!c.Enabled) c.Toggle();
                        }
                    }
                }

                Debug.Log("Loading Values...");
                if (json.TryGetValue("Values", out JToken valuesToken))
                {
                    foreach (var item in valuesToken.ToObject<Dictionary<string, string>>())
                    {
                        string s_cheat = item.Key;
                        string s_value = item.Value;

                        Cheat c = Cheat.instances.Find(c => c.GetType().Name == s_cheat);

                        if (c.GetType().GetInterface(typeof(IVariableCheat<>).FullName) != null)
                        {
                            FieldInfo valueField = c.GetType().GetField("Value", BindingFlags.Static | BindingFlags.Public);
                            valueField.SetValue(c, Convert.ChangeType(s_value, valueField.FieldType));
                        }
                    }
                }

                Debug.Log("Loading Cheat Settings...");
                if (json.TryGetValue("CheatSettings", out JToken cSettingsToken))
                {
                    JObject cheatSettings = cSettingsToken.ToObject<JObject>();
                    
                }

                Debug.Log("Loading Colors...");
                if (json.TryGetValue("Colors", out JToken colorsToken))
                {
                    JObject colors = colorsToken.ToObject<JObject>();

                    if (colors.TryGetValue("MenuText", out JToken menuTextToken))
                        c_menuText = JsonConvert.DeserializeObject<RGBAColor>(menuTextToken.ToString());
                    if (colors.TryGetValue("ESPPlayers", out JToken espPlayersToken))
                        c_espPlayers = JsonConvert.DeserializeObject<RGBAColor>(espPlayersToken.ToString());
                    if (colors.TryGetValue("ESPItems", out JToken espItemsToken))
                        c_espItems = JsonConvert.DeserializeObject<RGBAColor>(espItemsToken.ToString());
                    if (colors.TryGetValue("ESPEnemies", out JToken espMonstersToken))
                        c_espEnemies = JsonConvert.DeserializeObject<RGBAColor>(espMonstersToken.ToString());
                    if (colors.TryGetValue("ESPTraps", out JToken espDivingBellsToken))
                        c_espTraps = JsonConvert.DeserializeObject<RGBAColor>(espDivingBellsToken.ToString());
                    if (colors.TryGetValue("ChamPlayers", out JToken chamPlayersToken))
                        c_chamPlayers = JsonConvert.DeserializeObject<RGBAColor>(chamPlayersToken.ToString());
                    if (colors.TryGetValue("ChamItems", out JToken chamItemsToken))
                        c_chamItems = JsonConvert.DeserializeObject<RGBAColor>(espDivingBellsToken.ToString());
                    if (colors.TryGetValue("ChamMonsters", out JToken chamMonstersToken))
                        c_chamMonsters = JsonConvert.DeserializeObject<RGBAColor>(espDivingBellsToken.ToString());
                    if (colors.TryGetValue("ChamDivingBell", out JToken chamDivingBellToken))
                        c_chamDivingBell = JsonConvert.DeserializeObject<RGBAColor>(espDivingBellsToken.ToString());
                }

                Debug.Log("Loading Menu Settings...");
                if (json.TryGetValue("MenuSettings", out JToken settingsToken))
                {
                    JObject settings = settingsToken.ToObject<JObject>();

                    if (settings.TryGetValue("MenuFontSize", out JToken menuFontSizeToken))
                        i_menuFontSize = int.Parse(menuFontSizeToken.ToString());
                    if (settings.TryGetValue("MenuWidth", out JToken menuWidthToken))
                        i_menuWidth = int.Parse(menuWidthToken.ToString());
                    if (settings.TryGetValue("MenuHeight", out JToken menuHeightToken))
                        i_menuHeight = int.Parse(menuHeightToken.ToString());
                    if (settings.TryGetValue("SliderWidth", out JToken sliderWidthToken))
                        i_sliderWidth = int.Parse(sliderWidthToken.ToString());
                    if (settings.TryGetValue("TextboxWidth", out JToken textboxWidthToken))
                        i_textboxWidth = int.Parse(textboxWidthToken.ToString());
                    if (settings.TryGetValue("MenuAlpha", out JToken menuAlphaToken))
                        f_menuAlpha = float.Parse(menuAlphaToken.ToString());
                }

                Debug.Log("Loading Reaction Settings...");
                if (json.TryGetValue("Reactions", out JToken reactionsToken))
                {
                    JObject reactions = reactionsToken.ToObject<JObject>();

                    
                }
            }

            public static void RegenerateConfig()
            {
                if (HasConfig()) File.Delete(config);
                File.Copy(defaultConf, config);

                Cheat.instances.ForEach(c => c.keybind = c.defaultKeybind);

                LoadConfig();
            }
        }
    }
}
