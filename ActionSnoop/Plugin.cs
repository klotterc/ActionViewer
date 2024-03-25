using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.Command;
using Dalamud.Hooking;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;

namespace ActionSnoop
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "ActionSnoop";

        private const string commandName = "/snoop";

        [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] public static ITextureProvider TextureProvider { get; private set; } = null!;
        private Configuration Configuration { get; init; }
        private IActionSnoop ActionSnoop { get; init; }
        private PluginUI PluginUi { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {
            Services.Initialize(pluginInterface);

            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(Services.PluginInterface);

            ActionSnoop = new ActionSnoop();

            // you might normally want to embed resources and load them from the manifest stream
            PluginUi = new PluginUI(Configuration, ActionSnoop);

            Services.Commands.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Bring up list of nearby player essence and lost actions"
            });

            Services.PluginInterface.UiBuilder.Draw += DrawUI;
            //Services.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            PluginUi.Dispose();
            Services.Commands.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            this.PluginUi.Visible = !this.PluginUi.Visible;
        }

        private void DrawUI()
        {
            this.PluginUi.Draw();
        }

        //private void DrawConfigUI()
        //{
        //    this.PluginUi.SettingsVisible = true;
        //}
    }
}
