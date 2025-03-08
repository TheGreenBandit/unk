using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unk
{
    public class Loader : MonoBehaviour
    {
        private static GameObject Load;
        public static bool harmonyLoaded = false;

        public static void Init()
        {
            if (Load != null) return;
            LoadHarmony();
            Load = new GameObject();
            Load.AddComponent<Unk>();
            Object.DontDestroyOnLoad(Load);
        }

        public static void LoadHarmony()
        {
            String name = "Unk.Resources.0Harmony.dll";
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);
            byte[] rawAssembly = new byte[stream.Length];
            stream.Read(rawAssembly, 0, (int)stream.Length);

            AppDomain.CurrentDomain.Load(rawAssembly);
            harmonyLoaded = true;
        }
         
        public static void Unload() => Object.Destroy(Load);
    }
}
