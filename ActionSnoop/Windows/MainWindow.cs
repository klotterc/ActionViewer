using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Internal.Windows.Settings;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ActionSnoop.Tabs;

namespace ActionSnoop.Windows;

public class MainWindow : Window
{
    private Plugin plugin;
    private List<MainWindowTab> tabs;

    public MainWindow(Plugin plugin) : base("ActionSnoop")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(300, 300) * ImGuiHelpers.GlobalScale,
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        Size = new Vector2(600, 400);
        SizeCondition = ImGuiCond.FirstUseEver;

        this.plugin = plugin;
        this.tabs = new List<MainWindowTab> {
            new MainTab(this.plugin)
        };
    }

    public void Dispose()
    {
        this.tabs.Clear();
    }

    public override void Draw()
    {
        if (ImGui.BeginTabBar("##ActionSnoop_MainWindowTabs", ImGuiTabBarFlags.None))
        {
            foreach (var tab in tabs)
            {
                if (ImGui.BeginTabItem(tab.Name))
                {
                    tab.Draw();
                    ImGui.EndTabItem();
                }
            }

            ImGui.EndTabBar();
        }
    }
}