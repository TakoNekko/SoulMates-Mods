using HarmonyLib;
using System;
using System.Reflection;
using SoulMatesPinnedLineView = SoulMates.Dialogue.PinnedLineView;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(SoulMatesPinnedLineView), "SetVisible", new Type[] { typeof(bool) })]
	public class PinnedLineView_SetVisible
	{
		public static float fontSizeScale;

		private static float? fontSizeDefault;

		private static FieldInfo LineView_lineText;
		private static FieldInfo LineView_visible;

		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					LineView_lineText = typeof(SoulMatesPinnedLineView).GetField("_lineText", BindingFlags.NonPublic | BindingFlags.Instance);
					LineView_visible = typeof(SoulMatesPinnedLineView).GetField("_visible", BindingFlags.NonPublic | BindingFlags.Instance);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
					return false;
				}
			}

			return true;
		}

		public static void Postfix(SoulMatesPinnedLineView __instance)
		{
			ScaleFontSize(__instance);
		}

		public static void ScaleFontSize(SoulMatesPinnedLineView __instance)
		{
			var lineText = (TMP_Juicy)LineView_lineText.GetValue(__instance);
			var visible = (bool)LineView_visible.GetValue(__instance);

			if (visible)
			{
				if (!fontSizeDefault.HasValue)
				{
					fontSizeDefault = lineText.fontSize;
				}

				var fontSize = fontSizeDefault.Value * fontSizeScale;

				if (lineText.fontSizeMax < fontSize)
				{
					lineText.fontSizeMax = fontSize;
				}

				lineText.fontSize = fontSize;
			}
		}
	}
}
