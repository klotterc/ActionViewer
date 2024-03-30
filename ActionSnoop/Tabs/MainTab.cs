using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ActionSnoop;
using ActionSnoop.Enums;
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
        //ImGui.BeginChild("##action_snoop_internal", ImGuiHelpers.ScaledVector2(-1f, -25f));
        //ImGui.SetNextWindowSizeConstraints(new Vector2(UIMinWidth, UIMinHeight), new Vector2(UIMaxWidth, UIMaxHeight));
        //List<PlayerCharacter> playerCharacters = actionSnoop.getDummyList();
        List<PlayerCharacter> playerCharacters = this.Plugin.ActionSnoop.getPlayerCharacters();

        ImGuiTableFlags tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;// | ImGuiTableFlags.SizingFixedFit;
        var iconSize = ImGui.GetTextLineHeight() * 2f;
        var iconSizeVec = new Vector2(iconSize, iconSize);

        string playerName;
        uint jobId;
        StatusList statusList;
        int essenceId;
        int leftId = 0;
        int rightId = 0;

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

                uint essenceIconId = 26;
                uint leftIconId = 33;
                uint rightIconId = 33;

                List<int> essenceIds = new List<int>() { 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2434, 2435, 2436, 2437, 2438, 2439, };

                /* use status list to find:
                 * char essence ID (or -1)
                 * char reminiscence ID (or -1)
                 */

                foreach (Status status in statusList)
                {
                    int statusId = (int)status.StatusId;
                    if (essenceIds.Contains(statusId))
                    {
                        essenceId = statusId;

                        if (essenceId >= 2311 && essenceId <= 2325)
                        {
                            essenceIconId = 62386 + (uint)essenceId;
                        }
                        if (essenceId >= 2434 && essenceId <= 2439)
                        {
                            essenceIconId = 62293 + (uint)essenceId;
                        }
                    }
                    if (statusId.Equals(2348))
                    {
                        leftId = status.Param % 256;
                        rightId = (status.Param - leftId) / 256;

                        if (leftId == 71 || leftId == 72)
                        {
                            leftId += 6;
                        }
                        if (rightId == 71 || rightId == 72)
                        {
                            rightId += 6;
                        }

                        if (leftId > 0)
                        {
                            leftIconId = 64656 + (uint)leftId;
                        }
                        if (rightId > 0)
                        {
                            rightIconId = 64656 + (uint)rightId;
                        }
                    }
                }

                if (searchText == string.Empty ||
                    (leftIconId != 33 && ((BozjaActions)rightId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1) ||
                    (leftIconId != 33 && ((BozjaActions)leftId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1))
                {

                    // player job, name
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(62400 + jobId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    ImGui.TableNextColumn();
                    ImGui.Text(playerFirstLast[0] + "\n" + playerFirstLast[1]);

                    // essence
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(essenceIconId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);

                    // left/right actions
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(leftIconId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    ImGui.TableNextColumn();
                    ImGui.Image(Plugin.TextureProvider.GetIcon(rightIconId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                }
            }
            ImGui.EndTable();

        }
        //ImGui.EndChild();
    }
}