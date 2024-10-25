using HarmonyLib;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using SoulMatesLineView = SoulMates.Dialogue.LineView;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(SoulMatesLineView), "RunLine", new Type[] { typeof(Yarn.Unity.LocalizedLine), typeof(Action) })]
	public class LineView_RunLine
	{
		public static float fontSizeScale;

		public static float containerHeightScale;

		private static float? fontSizeDefault;

		private static float? containerHeightDefault;

		private static FieldInfo LineView_lineText;
		
		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					LineView_lineText = typeof(SoulMatesLineView).GetField("_lineText", BindingFlags.NonPublic | BindingFlags.Instance);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
					return false;
				}
			}

			return true;
		}

		public static void Postfix(SoulMatesLineView __instance)
		{
			ScaleFontSize(__instance);

			ScaleContainerHeight(__instance);
		}

		public static void ScaleFontSize(SoulMatesLineView __instance)
		{
			var lineText = (TMP_Juicy)LineView_lineText.GetValue(__instance);

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

		public static void ScaleContainerHeight(SoulMatesLineView __instance)
		{
			var container = (RectTransform)__instance.transform.parent;

			if (!containerHeightDefault.HasValue)
			{
				containerHeightDefault = container.sizeDelta.y;
			}

			container.sizeDelta = new Vector2(
				container.sizeDelta.x,
				containerHeightDefault.Value * containerHeightScale);
		}
	}
}
