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
        private const string CommandName = "/pmycommand";

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

            // you might normally want to embed resources and load them from the manifest stream

            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            WindowSystem.GetWindow("A Wonderful AFKPatcherConfig Window").IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("A Wonderful AFKPatcherConfig Window").IsOpen = true;
        }
    }
}
