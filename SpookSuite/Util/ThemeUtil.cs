﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Unk.Util
{
    public class ThemeUtil
    {
        public static string name { get; private set; }
        public static GUISkin Skin { get; private set; }
        public static AssetBundle AssetBundle;
        public static void Initialize() => SetTheme(string.IsNullOrEmpty(name) ? "Default" : name);
        public static void SetTheme(string t) => LoadTheme(name = ThemeExists(t) ? t : "Default");
        private static bool ThemeExists(string t) => Assembly.GetExecutingAssembly().GetManifestResourceStream($"SpookSuite.Resources.Theme.{t}.skin") != null;
        private static AssetBundle LoadAssetBundle(string r) => AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(r));
        public static string[] GetThemes() => Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(r => r.StartsWith("SpookSuite.Resources.Theme.") && r.EndsWith(".skin")).Select(r => r["SpookSuite.Resources.Theme.".Length..^".skin".Length]).OrderBy(name => name).ToArray();
        private static void LoadTheme(string t)
        {
            AssetBundle?.Unload(true);
            AssetBundle = null;
            Skin = null;
            AssetBundle = LoadAssetBundle($"SpookSuite.Resources.Theme.{t}.skin");
            if (AssetBundle == null) Debug.LogError($"[ERROR] Failed to load theme file => {$"SpookSuite.Resources.Theme.{t}.skin"}");
            Skin = AssetBundle.LoadAllAssets<GUISkin>().FirstOrDefault();
            if (Skin == null) return;
            Debug.Log($"Loaded Theme {t}");
        }
    }
}
