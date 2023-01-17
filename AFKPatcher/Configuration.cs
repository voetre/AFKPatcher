using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace AFKPatcher
{
    [Serializable]
    public class AFKPatcherConfig : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool InstanceKickerBool;
        public bool NoviceNetKickerBool;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
