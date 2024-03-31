using ActionViewer.Enums;
using ActionViewer.Models;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;

namespace ActionViewer.Functions
{
    public static class StatusInfoFunctions
    {
        private static List<int> essenceIds = new List<int>() { 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2434, 2435, 2436, 2437, 2438, 2439, };
        private static StatusInfo GetStatusInfo(StatusList statusList)
        {
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
                    statusInfo.rightId = (status.Param - statusInfo.leftId) / 256;

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
        private static List<CharRow> GenerateRows(List<PlayerCharacter> playerCharacters)
        {
            List<CharRow> charRowList = new List<CharRow>();
            foreach (PlayerCharacter character in playerCharacters)
            {
                // get player name, job ID, status list
                CharRow row = new CharRow();
                row.character = character;
                row.playerName = character.Name.ToString();
                row.jobId = (uint)character.ClassJob.GameData?.JobIndex;
                row.statusInfo = GetStatusInfo(character.StatusList);
                charRowList.Add(row);
            }
            return charRowList;
        }

        public static void GenerateStatusTable(List<PlayerCharacter> playerCharacters, string searchText, string filter = "none")
        {
            ImGuiTableFlags tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Sortable;// | ImGuiTableFlags.SizingFixedFit;
            var iconSize = ImGui.GetTextLineHeight() * 2f;
            var iconSizeVec = new Vector2(iconSize, iconSize);

            List<CharRow> charRowList = GenerateRows(playerCharacters);

            if (ImGui.BeginTable("table1", 5, tableFlags))
            {
                ImGui.TableSetupColumn("Job", ImGuiTableColumnFlags.WidthFixed, 34f, (int)charColumns.Job);
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.PreferSortDescending, 1f, (int)charColumns.Name);
                ImGui.TableSetupColumn("Ess.", ImGuiTableColumnFlags.WidthFixed, 34f, (int)charColumns.Essence);
                ImGui.TableSetupColumn("Left", ImGuiTableColumnFlags.WidthFixed, 34f, (int)charColumns.Left);
                ImGui.TableSetupColumn("Right", ImGuiTableColumnFlags.WidthFixed, 34f, (int)charColumns.Right);
                ImGui.TableHeadersRow();
                ImGuiTableSortSpecsPtr sortSpecs = ImGui.TableGetSortSpecs();
                charRowList = SortCharDataWithSortSpecs(sortSpecs, charRowList);

                foreach (CharRow row in charRowList)
                {

                    if ((searchText == string.Empty ||
                            (row.statusInfo.rightIconID != 33 && ((BozjaActions)row.statusInfo.rightId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1) ||
                            (row.statusInfo.leftIconID != 33 && ((BozjaActions)row.statusInfo.leftId).ToString().Replace("_", " ").ToLowerInvariant().IndexOf(searchText.ToLowerInvariant()) != -1)) && 
                        (filter == "none" || (filter == "noEss" && 
                            row.statusInfo.essenceIconID == 26))
                        )
                    {

                        // player job, name
                        ImGui.TableNextColumn();
                        //ImGui.Image(Plugin.TextureProvider.GetIcon(62400 + jobId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                        ImGui.Image(Plugin.TextureProvider.GetIcon(62400 + row.jobId)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                        ImGui.TableNextColumn();
                        ImGui.Selectable(row.playerName, false);
                        var hover = ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled);
                        var left = hover && ImGui.IsMouseClicked(ImGuiMouseButton.Left);
                        if (left)
                        {
                            Plugin.TargetManager.Target = row.character;
                        }

                        // essence
                        ImGui.TableNextColumn();
                        ImGui.Image(Plugin.TextureProvider.GetIcon(row.statusInfo.essenceIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);

                        // left/right actions
                        ImGui.TableNextColumn();
                        ImGui.Image(Plugin.TextureProvider.GetIcon(row.statusInfo.leftIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                        ImGui.TableNextColumn();
                        ImGui.Image(Plugin.TextureProvider.GetIcon(row.statusInfo.rightIconID)!.ImGuiHandle, iconSizeVec, Vector2.Zero, Vector2.One);
                    }
                }
                ImGui.EndTable();
            }
        }
        public enum charColumns
        {
            Job,
            Name,
            Essence,
            Left,
            Right
        }

        public static List<CharRow> SortCharDataWithSortSpecs(ImGuiTableSortSpecsPtr sortSpecs, List<CharRow> charDataList)
        {
            IEnumerable<CharRow> sortedCharaData = charDataList;

            for (int i = 0; i < sortSpecs.SpecsCount; i++)
            {
                ImGuiTableColumnSortSpecsPtr columnSortSpec = sortSpecs.Specs;

                switch ((charColumns)columnSortSpec.ColumnUserID)
                {
                    case charColumns.Job:
                        if (columnSortSpec.SortDirection == ImGuiSortDirection.Ascending)
                        {
                            sortedCharaData = sortedCharaData.OrderBy(o => o.jobId);
                        }
                        else
                        {
                            sortedCharaData = sortedCharaData.OrderByDescending(o => o.jobId);
                        }
                        break;
                    case charColumns.Name:
                        if (columnSortSpec.SortDirection == ImGuiSortDirection.Ascending)
                        {
                            sortedCharaData = sortedCharaData.OrderBy(o => o.playerName);
                        }
                        else
                        {
                            sortedCharaData = sortedCharaData.OrderByDescending(o => o.playerName);
                        }
                        break;
                    case charColumns.Essence:
                        if (columnSortSpec.SortDirection == ImGuiSortDirection.Ascending)
                        {
                            sortedCharaData = sortedCharaData.OrderBy(o => o.statusInfo.essenceIconID);
                        }
                        else
                        {
                            sortedCharaData = sortedCharaData.OrderByDescending(o => o.statusInfo.essenceIconID);
                        }
                        break;
                    case charColumns.Left:
                        if (columnSortSpec.SortDirection == ImGuiSortDirection.Ascending)
                        {
                            sortedCharaData = sortedCharaData.OrderBy(o => o.statusInfo.leftIconID);
                        }
                        else
                        {
                            sortedCharaData = sortedCharaData.OrderByDescending(o => o.statusInfo.leftIconID);
                        }
                        break;
                    case charColumns.Right:
                        if (columnSortSpec.SortDirection == ImGuiSortDirection.Ascending)
                        {
                            sortedCharaData = sortedCharaData.OrderBy(o => o.statusInfo.rightIconID);
                        }
                        else
                        {
                            sortedCharaData = sortedCharaData.OrderByDescending(o => o.statusInfo.rightIconID);
                        }
                        break;
                    default:
                        break;
                }
            }

            return sortedCharaData.ToList();
        }
    }
}
