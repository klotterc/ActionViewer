﻿using ActionSnoop.Functions;
using ActionSnoop.Windows;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace ActionSnoop.Tabs;

public class HealerTab : MainWindowTab
{
    private string searchText = string.Empty;
    private List<uint> healers = new List<uint>() { 6, 9, 13, 20 };

    public HealerTab(Plugin plugin) : base("Healers", plugin) { }

    public override void Draw()
    {
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters().Where(x => healers.Contains((uint)x.ClassJob.GameData?.JobIndex)).ToList();

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        StatusInfoFunctions.GenerateStatusTable(playerCharacters, searchText);
    }
}