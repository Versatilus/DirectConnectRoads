namespace DirectConnectRoads.LifeCycle
{
    using System;
    using JetBrains.Annotations;
    using ICities;
    using CitiesHarmony.API;
    using DirectConnectRoads.Util;
    public class Mod : IUserMod
    {
        public static Version ModVersion => typeof(Mod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Direct Connect Roads"+ VersionString;
        public string Description => "uses Direct Connect textures if TMPE rules suggests unbroken median";
        public static bool IsEnabled = false;

        [UsedImplicitly]
        public void OnEnabled()
        {
            HarmonyHelper.EnsureHarmonyInstalled();   
            if (Extensions.InGame)
                LifeCycle.Load();
            IsEnabled = true;
        }

        [UsedImplicitly]
        public void OnDisabled()
        {
            IsEnabled = false;
            LifeCycle.Unload();
        }
    }
}
