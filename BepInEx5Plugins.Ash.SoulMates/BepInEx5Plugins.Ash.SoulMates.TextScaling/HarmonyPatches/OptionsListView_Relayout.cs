using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Yarn.Unity;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(OptionsListView), "Relayout")]
	public class OptionsListView_Relayout
	{
		public static float fontSizeScale;

		private static float? fontSizeDefault;

		private static FieldInfo OptionsListView_optionViews;
		private static FieldInfo OptionView_text;

		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					OptionsListView_optionViews = typeof(OptionsListView).GetField("optionViews", BindingFlags.NonPublic | BindingFlags.Instance);
					OptionView_text = typeof(OptionView).GetField("text", BindingFlags.NonPublic | BindingFlags.Instance);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
					return false;
				}
			}

			return true;
		}

		public static void Prefix(OptionsListView __instance)
		{
			ScaleFontSize(__instance);
		}

		public static void ScaleFontSize(OptionsListView __instance)
		{
			var optionViews = (List<OptionView>)OptionsListView_optionViews.GetValue(__instance);

			foreach (var optionView in optionViews)
			{
				var text = (TextMeshProUGUI)OptionView_text.GetValue(optionView);

				if (!fontSizeDefault.HasValue)
				{
					fontSizeDefault = text.fontSize;
				}

				var fontSize = fontSizeDefault.Value * fontSizeScale;

				if (text.fontSizeMax < fontSize)
				{
					text.fontSizeMax = fontSize;
				}

				text.fontSize = fontSize;
			}
		}
	}
}
