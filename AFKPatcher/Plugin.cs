using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using AFKPatcher.Windows;

namespace AFKPatcher
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "AFK Patcher";
        private const string CommandName = "/afkconfig";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public AFKPatcherConfig AFKPatcherConfig { get; init; }
        public WindowSystem WindowSystem = new("AFKPatcher");

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.AFKPatcherConfig = this.PluginInterface.GetPluginConfig() as AFKPatcherConfig ?? new AFKPatcherConfig();
            this.AFKPatcherConfig.Initialize(this.PluginInterface);

            DalamudApi.Initialize(this, pluginInterface);


            WindowSystem.AddWindow(new ConfigWindow(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Plugin to patch the afk kickers/timers."
            });
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            if (AFKPatcherConfig.InstanceKickerBool) { ConfigWindow.InstanceKickerPatch.Enable(); }
            if (AFKPatcherConfig.NoviceNetKickerBool) { ConfigWindow.NoviceNetKickerPatch.Enable(); }

        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
            Memory.Dispose();
        }

        private void OnCommand(string command, string args)
        {
            WindowSystem.GetWindow("AFK Patcher Config").IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("AFK Patcher Config").IsOpen = true;
        }
    }
}
