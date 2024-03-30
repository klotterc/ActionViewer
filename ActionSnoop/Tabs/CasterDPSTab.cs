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

public class CasterDPSTab : MainWindowTab
{
    private string searchText = string.Empty;
    private List<uint> casterDPS = new List<uint>() { 7, 8, 15 };

    public CasterDPSTab(Plugin plugin) : base("Caster DPS", plugin) { }

    public override void Draw()
    {
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters().Where(x => casterDPS.Contains((uint)x.ClassJob.GameData?.JobIndex)).ToList();

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        StatusInfoFunctions.GenerateStatusTable(playerCharacters, searchText);
    }
}