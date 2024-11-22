﻿using ActionViewer.Tabs;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;

namespace ActionViewer.Windows;

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
	private List<ushort> territoryTypes = new List<ushort>() { 920, 936, 937, 975, 795, 827 };
	private static List<ushort> eurekaTerritories = new List<ushort>() { 795, 827 };
	private List<uint> tanks = new List<uint>() { 1, 3, 12, 17 };
	private List<uint> healers = new List<uint>() { 6, 9, 13, 20 };
	private List<uint> casterDPS = new List<uint>() { 7, 8, 15, 22 };
	private List<uint> melee = new List<uint>() { 2, 4, 10, 14, 19, 21 };
	private List<uint> physRanged = new List<uint>() { 5, 11, 18 };

	public MainWindow(Plugin plugin) : base("ActionViewer")
	{
		SizeConstraints = new WindowSizeConstraints
		{
			MinimumSize = new Vector2(300, 300) * ImGuiHelpers.GlobalScale,
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};
		Size = new Vector2(310, 200);
		SizeCondition = ImGuiCond.FirstUseEver;

		this.plugin = plugin;
		if (eurekaTerritories.Contains(Services.ClientState.TerritoryType))
		{
			this.tabs = new List<MainWindowTab> {
			new GeneratedTab(this.plugin, "Main"),
			new GeneratedTab(this.plugin, "Tanks", tanks),
			new GeneratedTab(this.plugin, "Healers", healers),
			new GeneratedTab(this.plugin, "Melee", melee),
			new GeneratedTab(this.plugin, "Phys Ranged", physRanged),
			new GeneratedTab(this.plugin, "Caster", casterDPS),
			};
		} else {
			this.tabs = new List<MainWindowTab> {
			new GeneratedTab(this.plugin, "Main"),
			new GeneratedTab(this.plugin, "No Ess."),
			new GeneratedTab(this.plugin, "Tanks", tanks),
			new GeneratedTab(this.plugin, "Healers", healers),
			new GeneratedTab(this.plugin, "Melee", melee),
			new GeneratedTab(this.plugin, "Phys Ranged", physRanged),
			new GeneratedTab(this.plugin, "Caster", casterDPS),
			};
		}
	}

	public void Dispose()
	{
		this.tabs.Clear();
	}

	public override void Draw()
	{
		if (territoryTypes.Contains(Services.ClientState.TerritoryType))
		{
			if (ImGui.BeginTabBar("##ActionViewer_MainWindowTabs", ImGuiTabBarFlags.None))
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
			ImGui.Text("Please Enter a Save the Queen or Eureka Zone");
		}
	}
}