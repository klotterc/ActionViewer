using ActionSnoop.Enums;
using ActionSnoop.Models;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Statuses;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ActionSnoop.Functions
{
    public static class StatusInfoFunctions
    {
        private static List<int> essenceIds = new List<int>() { 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2434, 2435, 2436, 2437, 2438, 2439, };
        private static StatusInfo GetStatusInfo(StatusList statusList)
        {
            int leftId = 0;
            StatusInfo statusInfo = new StatusInfo();

            foreach (Status status in statusList)
            {
                int statusId = (int)status.StatusId;
                if (essenceIds.Contains(statusId))
                {
                    statusInfo.essenceId = statusId;
                }
                if (statusId.Equals(2348))
                {
                    statusInfo.leftId = status.Param % 256;
                    statusInfo.rightId = (status.Param - leftId) / 256;

                    if (statusInfo.leftId == 71 || statusInfo.leftId == 72)
                    {
                        statusInfo.leftId += 6;
                    }
                    if (statusInfo.rightId == 71 || statusInfo.rightId == 72)
                    {
                        statusInfo.rightId += 6;
                    }
                }
            }
            return statusInfo;
        }

        public static void GenerateStatusTable(List<PlayerCharacter> playerCharacters, string searchText)
        {
            ImGuiTableFlags tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;// | ImGuiTableFlags.SizingFixedFit;
            var iconSize = ImGui.GetTextLineHeight() * 2f;
            var iconSizeVec = new Vector2(iconSize, iconSize);

            string playerName;
            uint jobId;
            StatusList statusList;

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
        }
    }
}
