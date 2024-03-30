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

public class TankTab : MainWindowTab
{
    private string searchText = string.Empty;
    private List<uint> tanks = new List<uint>() { 1, 3, 12, 17 };

    public TankTab(Plugin plugin) : base("Tanks", plugin) { }

    public override void Draw()
    {
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters().Where(x => tanks.Contains((uint)x.ClassJob.GameData?.JobIndex)).ToList();

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        StatusInfoFunctions.GenerateStatusTable(playerCharacters, searchText);
    }
}