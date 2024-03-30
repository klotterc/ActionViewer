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
        //ImGui.BeginChild("##action_snoop_internal", ImGuiHelpers.ScaledVector2(-1f, -25f));
        //ImGui.SetNextWindowSizeConstraints(new Vector2(UIMinWidth, UIMinHeight), new Vector2(UIMaxWidth, UIMaxHeight));
        //List<PlayerCharacter> playerCharacters = actionSnoop.getDummyList();
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters().Where(x => casterDPS.Contains((uint)x.ClassJob.GameData?.JobIndex)).ToList();

        ImGuiTableFlags tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;// | ImGuiTableFlags.SizingFixedFit;
        var iconSize = ImGui.GetTextLineHeight() * 2f;
        var iconSizeVec = new Vector2(iconSize, iconSize);

        string playerName;
        uint jobId;
        StatusList statusList;

        ImGui.SetNextItemWidth(-1 * ImGui.GetIO().FontGlobalScale);
        ImGui.InputText("", ref searchText, 256);
        if (ImGui.BeginTable("table1", 5, tableFlags))
        {
            ImGui.TableSetupColumn("Job", ImGuiTableColumnFlags.WidthFixed, 34f);
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("Ess.", ImGuiTableColumnFlags.WidthFixed, 34f);
            ImGui.TableSetupColumn("Left", ImGuiTableColumnFlags.WidthFixed, 34f);
            ImGui.TableSetupColumn("Right", ImGuiTableColumnFlags.WidthFixed, 34f);
            ImGui.TableHeadersRow();
            foreach (PlayerCharacter character in playerCharacters)
            {
                // get player name, job ID, status list
                playerName = character.Name.ToString();
                String[] playerFirstLast = playerName.Split(" ");
                jobId = (uint)character.ClassJob.GameData?.JobIndex;
                statusList = character.StatusList;

                /* use status list to find:
                 * char essence ID (or -1)
                 * char reminiscence ID (or -1)
                 */

                StatusInfo statusInfo = StatusInfoFunctions.GetStatusInfo(statusList);

                if (searchText == string.Empty ||
                    (statusInfo.rightIconID != 33 && ((BozjaActions)statusInfo.rightId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1) ||
                    (statusInfo.leftIconID != 33 && ((BozjaActions)statusInfo.leftId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1))
                {

                    // player job, name
                    ImGui.TableNextColumn();
                    //ImGui.Image(Plugin.TextureProvider.GetIcon(62400 + jobId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    ImGui.Image(Plugin.TextureProvider.GetIcon(62400 + jobId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    ImGui.TableNextColumn();
                    ImGui.Text(playerFirstLast[0] + "\n" + playerFirstLast[1]);

                    // essence
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(statusInfo.essenceIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);

                    // left/right actions
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(statusInfo.leftIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(statusInfo.rightIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                }
            }
            ImGui.EndTable();

        }
        //ImGui.EndChild();
    }
}