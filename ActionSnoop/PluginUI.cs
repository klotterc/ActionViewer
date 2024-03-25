
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Statuses;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Numerics;
using Status = Dalamud.Game.ClientState.Statuses.Status;

namespace ActionSnoop
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private string searchText = string.Empty;
        const float UIMinWidth = 310;
        const float UIMinHeight = 100;
        const float UIMaxWidth = 335;
        const float UIMaxHeight = 670;

        private readonly Configuration configuration;

        private readonly IActionSnoop actionSnoop;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration, IActionSnoop actionSnoop)
        {
            this.configuration = configuration;
            this.actionSnoop = actionSnoop;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            DrawMainWindow();
            //DrawSettingsWindow();
        }

        public void DrawMainWindow()
        {
            if (!Visible)
            {
                return;
            }

            //ImGui.SetNextWindowSize(new Vector2(UIWidth, UIHeight), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(UIMinWidth, UIMinHeight), new Vector2(UIMaxWidth, UIMaxHeight));
            //List<PlayerCharacter> playerCharacters = actionSnoop.getDummyList();
            List<PlayerCharacter> playerCharacters = actionSnoop.getPlayerCharacters();

            ImGuiTableFlags tableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg;// | ImGuiTableFlags.SizingFixedFit;
            var iconSize = ImGui.GetTextLineHeight() * 2f;
            var iconSizeVec = new Vector2(iconSize, iconSize);

            string playerName;
            uint jobId;
            StatusList statusList;
            int essenceId;
            int leftId = 0;
            int rightId = 0;


            if (ImGui.Begin("Action Snooper", ref visible, ImGuiWindowFlags.AlwaysAutoResize))
            {
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
            }
            ImGui.End();
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
            if (ImGui.Begin("Mimicry Helper Configuration", ref this.settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                // make configuration here (reference git history or SamplePlugin for original example)

                // can save immediately on change, if you don't want to provide a "Save and Close" button
                this.configuration.Save();
            }
            ImGui.End();
        }
    }
}