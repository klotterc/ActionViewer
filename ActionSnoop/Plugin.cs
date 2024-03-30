using ActionSnoop.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace ActionSnoop
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "ActionSnoop";

        private const string commandName = "/snoop";

        [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] public static ITextureProvider TextureProvider { get; private set; } = null!;

        public readonly WindowSystem WindowSystem = new("ActionSnoop");
        public readonly MainWindow MainWindow;
        private Configuration Configuration { get; init; }
        public IActionSnoop ActionSnoop { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {
            Services.Initialize(pluginInterface);

            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(Services.PluginInterface);

            this.MainWindow = new MainWindow(this);
            this.WindowSystem.AddWindow(this.MainWindow);

            PluginInterface.UiBuilder.Draw += this.DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += this.DrawConfigUI;

            ActionSnoop = new ActionSnoop();

            // you might normally want to embed resources and load them from the manifest stream
            //PluginUi = new PluginUI(Configuration, ActionSnoop);

            Services.Commands.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Bring up list of nearby player essence and lost actions"
            });

            Services.PluginInterface.UiBuilder.Draw += DrawUI;
            //Services.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            WindowSystem.RemoveAllWindows();
            Services.Commands.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            this.MainWindow.IsOpen ^= true;
        }

        private void DrawUI()
        {
            WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            this.MainWindow.IsOpen = true;
        }

        //private void DrawConfigUI()
        //{
        //    this.PluginUi.SettingsVisible = true;
        //}
    }
}
