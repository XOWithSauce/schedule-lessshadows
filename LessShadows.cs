using MelonLoader;
using System.Collections;
using UnityEngine;
using Il2CppScheduleOne.Persistence;

[assembly: MelonInfo(typeof(LessShadows.LessShadows), LessShadows.BuildInfo.Name, LessShadows.BuildInfo.Version, LessShadows.BuildInfo.Author, LessShadows.BuildInfo.DownloadLink)]
[assembly: MelonColor()]
[assembly: MelonOptionalDependencies("FishNet.Runtime")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace LessShadows
{
    public static class BuildInfo
    {
        public const string Name = "LessShadows";
        public const string Description = "LessShadowss";
        public const string Author = "XOWithSauce";
        public const string Company = null;
        public const string Version = "1.0";
        public const string DownloadLink = null;
    }

    public class LessShadows : MelonMod
    {
        public static List<object> coros = new();
        private bool registered = false;
        private void OnLoadCompleteCb()
        {
            if (registered) return;
            coros.Add(MelonCoroutines.Start(Setup()));
            registered = true;
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == 1)
            {
                // MelonLogger.Msg("Start State");
                if (LoadManager.Instance != null && !registered)
                {
                    LoadManager.Instance.onLoadComplete.AddListener((UnityEngine.Events.UnityAction)OnLoadCompleteCb);
                }
            }
            else
            {
                registered = false;
                // MelonLogger.Msg("Clear State");
                foreach (object coro in coros)
                {
                    MelonCoroutines.Stop(coro);
                }
                coros.Clear();
            }
        }

        public static IEnumerator Setup()
        {
            yield return new WaitForSeconds(10f);
            Light[] lights = UnityEngine.Object.FindObjectsOfType<Light>(true);
            MelonLogger.Msg("DISABLING SHADOWS FOR " + lights.Length + " LIGHTS!");
            MelonLogger.Msg("YOU MIGHT EXPERIENCE SLIGHT LAG DURING THIS OPERATION!");
            foreach (Light light in lights) {
                yield return new WaitForSeconds(0.001f);
                yield return new WaitForEndOfFrame();
                light.shadows = LightShadows.None;
            }
            MelonLogger.Msg("LIGHT SHADOWS HAVE BEEN DISABLED. ENJOY YOUR FPS!");
            yield return null;
        }
    }
}
