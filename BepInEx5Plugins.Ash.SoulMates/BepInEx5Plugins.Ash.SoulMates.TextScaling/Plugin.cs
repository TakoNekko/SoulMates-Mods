using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SoulMates.Inventory;
using System;
using Yarn.Unity;
using SoulMatesLineView = SoulMates.Dialogue.LineView;
using SoulMatesPinnedLineView = SoulMates.Dialogue.PinnedLineView;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private ConfigEntry<float> lineViewTextFontSizeScale;

		private ConfigEntry<float> lineViewContainerHeightScale;

		private ConfigEntry<float> lineViewArrowOffsetY;

		private ConfigEntry<float> lineViewCharacterNameFontSizeScale;
		private ConfigEntry<float> lineViewCharacterNameContainerWidthScale;
		private ConfigEntry<float> lineViewCharacterNameContainerHeightScale;
		private ConfigEntry<float> lineViewCharacterNameContainerOffsetY;

		private ConfigEntry<float> pinnedLineViewTextFontSizeScale;

		private ConfigEntry<float> optionsListViewTextFontSizeScale;

		private ConfigEntry<float> inventoryItemTooltipFontSizeScale;

		private ConfigEntry<float> cursorTooltipFontSizeScale;

		private Plugin()
		{
			lineViewTextFontSizeScale = Config.Bind("Line View", "Text Font Size Scale", 2f);

			lineViewContainerHeightScale = Config.Bind("Line View", "Container Height Scale", 1.15f);

			lineViewArrowOffsetY = Config.Bind("Line View", "Arrow Offset Y", 1.25f);

			lineViewCharacterNameFontSizeScale = Config.Bind("Line View", "Character Name Font Size Scale", 2f);
			lineViewCharacterNameContainerWidthScale = Config.Bind("Line View", "Character Name Container Width Scale", 1f);
			lineViewCharacterNameContainerHeightScale = Config.Bind("Line View", "Character Name Container Height Scale", 2f);
			lineViewCharacterNameContainerOffsetY = Config.Bind("Line View", "Character Name Container Offset Y", 1.75f);

			pinnedLineViewTextFontSizeScale = Config.Bind("Pinned Line View", "Text Font Size Scale", 1.5f);

			optionsListViewTextFontSizeScale = Config.Bind("Options List View", "Text Font Size Scale", 1.5f);

			inventoryItemTooltipFontSizeScale = Config.Bind("Inventory Item", "Tooltip Font Size Scale", 1.5f);

			cursorTooltipFontSizeScale = Config.Bind("Cursor", "Tooltip Font Size Scale", 2.5f);

			lineViewTextFontSizeScale.SettingChanged += LineViewTextFontSizeScale_SettingChanged;

			lineViewContainerHeightScale.SettingChanged += LineViewContainerHeightScale_SettingChanged;

			lineViewArrowOffsetY.SettingChanged += LineViewArrowOffsetY_SettingChanged;

			lineViewCharacterNameFontSizeScale.SettingChanged += LineViewCharacterNameFontSizeScale_SettingChanged;
			lineViewCharacterNameContainerWidthScale.SettingChanged += LineViewCharacterNameContainerWidthScale_SettingChanged;
			lineViewCharacterNameContainerHeightScale.SettingChanged += LineViewCharacterNameContainerHeightScale_SettingChanged;
			lineViewCharacterNameContainerOffsetY.SettingChanged += LineViewCharacterNameContainerOffsetY_SettingChanged;

			pinnedLineViewTextFontSizeScale.SettingChanged += PinnedLineViewTextFontSizeScale_SettingChanged;

			optionsListViewTextFontSizeScale.SettingChanged += OptionsListViewTextFontSizeScale_SettingChanged;

			inventoryItemTooltipFontSizeScale.SettingChanged += InventoryItemTooltipFontSizeScale_SettingChanged;

			cursorTooltipFontSizeScale.SettingChanged += CursorTooltipFontSizeScale_SettingChanged;
		}

		private void Awake()
		{
			try
			{
				Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

				var harmony = new Harmony(Info.Metadata.GUID);

				HarmonyPatches.LineView_RunLine.fontSizeScale = lineViewTextFontSizeScale.Value;

				HarmonyPatches.LineView_RunLine.containerHeightScale = lineViewContainerHeightScale.Value;

				HarmonyPatches.LineView_UserRequestedViewAdvancement.arrowOffsetY = lineViewArrowOffsetY.Value;

				HarmonyPatches.LineView_RunLine.characterNameFontSizeScale = lineViewCharacterNameFontSizeScale.Value;
				HarmonyPatches.LineView_RunLine.characterNameContainerWidthScale = lineViewCharacterNameContainerWidthScale.Value;
				HarmonyPatches.LineView_RunLine.characterNameContainerHeightScale = lineViewCharacterNameContainerHeightScale.Value;
				HarmonyPatches.LineView_RunLine.characterNameContainerOffsetY = lineViewCharacterNameContainerOffsetY.Value;

				HarmonyPatches.PinnedLineView_SetVisible.fontSizeScale = pinnedLineViewTextFontSizeScale.Value;

				HarmonyPatches.OptionsListView_Relayout.fontSizeScale = optionsListViewTextFontSizeScale.Value;

				HarmonyPatches.InventoryItem_AttachRoomItem.fontSizeScale = inventoryItemTooltipFontSizeScale.Value;

				HarmonyPatches.Cursor_DrawTooltip.fontSizeScale = cursorTooltipFontSizeScale.Value;

				harmony.PatchAll();
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		private void LineViewTextFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.fontSizeScale = lineViewTextFontSizeScale.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.ScaleFontSize(lineView);
			}
		}

		private void LineViewContainerHeightScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.containerHeightScale = lineViewContainerHeightScale.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.ScaleContainerHeight(lineView);
			}
		}

		private void LineViewArrowOffsetY_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_UserRequestedViewAdvancement.arrowOffsetY = lineViewArrowOffsetY.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_UserRequestedViewAdvancement.AdjustArrowSpringPositionGoal(lineView);
			}
		}

		private void LineViewCharacterNameFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.characterNameFontSizeScale = lineViewCharacterNameFontSizeScale.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.ScaleCharacterNameFontSize(lineView);
			}
		}

		private void LineViewCharacterNameContainerWidthScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.characterNameContainerWidthScale = lineViewCharacterNameContainerWidthScale.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.ScaleCharacterNameContainerSize(lineView);
			}
		}

		private void LineViewCharacterNameContainerHeightScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.characterNameContainerHeightScale = lineViewCharacterNameContainerHeightScale.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.ScaleCharacterNameContainerSize(lineView);
			}
		}

		private void LineViewCharacterNameContainerOffsetY_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.LineView_RunLine.characterNameContainerOffsetY = lineViewCharacterNameContainerOffsetY.Value;

			foreach (var lineView in FindObjectsOfType<SoulMatesLineView>())
			{
				HarmonyPatches.LineView_RunLine.AdjustCharacterNameContainerOffset(lineView);
			}
		}

		private void PinnedLineViewTextFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.PinnedLineView_SetVisible.fontSizeScale = pinnedLineViewTextFontSizeScale.Value;

			foreach (var pinnedLineView in FindObjectsOfType<SoulMatesPinnedLineView>())
			{
				HarmonyPatches.PinnedLineView_SetVisible.ScaleFontSize(pinnedLineView);
			}
		}

		private void OptionsListViewTextFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.OptionsListView_Relayout.fontSizeScale = pinnedLineViewTextFontSizeScale.Value;

			foreach (var optionsListView in FindObjectsOfType<OptionsListView>())
			{
				HarmonyPatches.OptionsListView_Relayout.ScaleFontSize(optionsListView);
			}
		}

		private void InventoryItemTooltipFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.InventoryItem_AttachRoomItem.fontSizeScale = inventoryItemTooltipFontSizeScale.Value;

			foreach (var inventoryItem in FindObjectsOfType<InventoryItem>())
			{
				HarmonyPatches.InventoryItem_AttachRoomItem.ScaleFontSize(inventoryItem);
			}
		}

		private void CursorTooltipFontSizeScale_SettingChanged(object sender, EventArgs e)
		{
			HarmonyPatches.Cursor_DrawTooltip.fontSizeScale = cursorTooltipFontSizeScale.Value;
		}
	}
}
