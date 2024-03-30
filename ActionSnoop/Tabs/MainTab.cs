﻿using ActionSnoop.Functions;
using ActionSnoop.Windows;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ImGuiNET;
using System.Collections.Generic;

namespace ActionSnoop.Tabs;

public class MainTab : MainWindowTab
{
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