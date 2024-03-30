using ActionSnoop.Tabs;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;

namespace ActionSnoop.Windows;

public class MainWindow : Window
{
    private Plugin plugin;
    private List<MainWindowTab> tabs;
    /*
     * 920 - BSF
     * 936 - DRN
     * 937 - DRS
     * 975 - Zadnor
     */
    private List<ushort> territoryTypes = new List<ushort>() { 920, 936, 937, 975 };

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
            new MainTab(this.plugin),
            new TankTab(this.plugin),
            new HealerTab(this.plugin),
            new MeleeTab(this.plugin),
            new PhysRangedTab(this.plugin),
            new CasterDPSTab(this.plugin),
            new NoEssenceTab(this.plugin)
        };
    }

    public void Dispose()
    {
        this.tabs.Clear();
    }

    public override void Draw()
    {
        if (territoryTypes.Contains(Services.ClientState.TerritoryType))
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
        else
        {
            ImGui.Text("Please Enter a Save the Queen Zone");
        }
    }
}