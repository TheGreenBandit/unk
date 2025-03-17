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
        //private ID3D11Device device;
        //private ID3D11DeviceContext context;
        //private ID3D11RenderTargetView renderTargetView;
        //private ImGuiRenderer imguiRenderer;

        public static void Init()
        {
            ImGuiNET.ImGui.Begin("main");
            ImGuiNET.ImGui.LabelText("hi", "");
            ImGuiNET.ImGui.End();
            if (Load != null)
            {
                Debug.LogError("Unk is already injected");
                return;
            }
            
            LoadHarmony();
            Loader.Load = new GameObject();
            Load.AddComponent<Unk>();
            Object.DontDestroyOnLoad(Loader.Load);
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
