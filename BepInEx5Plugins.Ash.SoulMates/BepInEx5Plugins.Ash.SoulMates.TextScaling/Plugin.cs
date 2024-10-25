using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using SoulMatesLineView = SoulMates.Dialogue.LineView;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private ConfigEntry<float> lineViewTextFontSizeScale;

		private Plugin()
		{
			lineViewTextFontSizeScale = Config.Bind("Line View", "Text Font Size Scale", 2f);

			lineViewTextFontSizeScale.SettingChanged += LineViewTextFontSizeScale_SettingChanged;
		}

		private void Awake()
		{
			try
			{
				Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

				var harmony = new Harmony(Info.Metadata.GUID);

				HarmonyPatches.LineView_RunLine.fontSizeScale = lineViewTextFontSizeScale.Value;

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
	}
}
