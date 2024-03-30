using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ActionSnoop;
using ActionSnoop.Enums;
using ActionSnoop.Functions;
using ActionSnoop.Models;
using ActionSnoop.Windows;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Statuses;
using Dalamud.Interface.Utility;
using Dalamud.Logging;
using Dalamud.Utility;
using ImGuiNET;

namespace ActionSnoop.Tabs;

public class MainTab : MainWindowTab {
    private string searchText = string.Empty;

    public MainTab(Plugin plugin) : base("Main", plugin) { }

    public override void Draw()
    {
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters();

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        StatusInfoFunctions.GenerateStatusTable(playerCharacters, searchText);
    }
}