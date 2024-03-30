﻿using ActionSnoop.Functions;
using ActionSnoop.Windows;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace ActionSnoop.Tabs;

public class MeleeTab : MainWindowTab
{
    private string searchText = string.Empty;
    private List<uint> physDPS = new List<uint>() { 2, 4, 10, 14, 19 };

    public MeleeTab(Plugin plugin) : base("Melee", plugin) { }

    public override void Draw()
    {
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters().Where(x => physDPS.Contains((uint)x.ClassJob.GameData?.JobIndex)).ToList();

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        StatusInfoFunctions.GenerateStatusTable(playerCharacters, searchText);
    }
}