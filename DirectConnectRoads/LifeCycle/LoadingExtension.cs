namespace DirectConnectRoads.LifeCycle
{
    using ICities;
    using DirectConnectRoads.Util;

    public class LoadingExtention : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            Log.Debug("LoadingExtention.OnLevelLoaded");
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario)
                LifeCycle.Load();
        }

        public override void OnLevelUnloading()
        {
            Log.Debug("LoadingExtention.OnLevelUnloading");
            LifeCycle.Unload();
        }
    }
}
