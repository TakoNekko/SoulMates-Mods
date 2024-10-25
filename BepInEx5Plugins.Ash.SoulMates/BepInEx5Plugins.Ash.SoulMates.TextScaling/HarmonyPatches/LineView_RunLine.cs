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

		public static float characterNameFontSizeScale;
		public static float characterNameContainerWidthScale;
		public static float characterNameContainerHeightScale;
		public static float characterNameContainerOffsetY;

		private static float? fontSizeDefault;

		private static float? containerHeightDefault;

		private static float? characterNameFontSizeDefault;
		private static Vector2? characterNameContainerSizeDeltaDefault;
		private static float? characterNameContainerOffsetYDefault;

		private static FieldInfo LineView_lineText;

		private static FieldInfo LineView_characterNameContainer;
		private static FieldInfo LineView_characterNameText;
		
		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					LineView_lineText = typeof(SoulMatesLineView).GetField("_lineText", BindingFlags.NonPublic | BindingFlags.Instance);

					LineView_characterNameContainer = typeof(SoulMatesLineView).GetField("_characterNameContainer", BindingFlags.NonPublic | BindingFlags.Instance);
					LineView_characterNameText = typeof(SoulMatesLineView).GetField("_characterNameText", BindingFlags.NonPublic | BindingFlags.Instance);
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

			LineView_UserRequestedViewAdvancement.AdjustArrowSpringPositionGoal(__instance);

			ScaleCharacterNameFontSize(__instance);
			ScaleCharacterNameContainerSize(__instance);
			AdjustCharacterNameContainerOffset(__instance);
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

		public static void ScaleCharacterNameFontSize(SoulMatesLineView __instance)
		{
			var characterNameText = (TextMeshProUGUI)LineView_characterNameText.GetValue(__instance);

			if (!characterNameFontSizeDefault.HasValue)
			{
				characterNameFontSizeDefault = characterNameText.fontSize;
			}

			var fontSize = characterNameFontSizeDefault.Value * characterNameFontSizeScale;

			if (characterNameText.fontSizeMax < fontSize)
			{
				characterNameText.fontSizeMax = fontSize;
			}

			characterNameText.fontSize = fontSize;
		}

		public static void ScaleCharacterNameContainerSize(SoulMatesLineView __instance)
		{
			var characterNameContainer = (RectTransform)((GameObject)LineView_characterNameContainer.GetValue(__instance)).transform;

			if (!characterNameContainerSizeDeltaDefault.HasValue)
			{
				characterNameContainerSizeDeltaDefault = characterNameContainer.sizeDelta;
			}

			characterNameContainer.sizeDelta = new Vector2(
				characterNameContainerSizeDeltaDefault.Value.x * characterNameContainerWidthScale,
				characterNameContainerSizeDeltaDefault.Value.y * characterNameContainerHeightScale);

			if (!characterNameContainerOffsetYDefault.HasValue)
			{
				characterNameContainerOffsetYDefault = characterNameContainer.anchoredPosition.y;
			}

			if (characterNameContainerHeightScale != 0f)
			{
				characterNameContainer.anchoredPosition = new Vector2(
					characterNameContainer.anchoredPosition.x,
					characterNameContainerOffsetYDefault.Value + (characterNameContainerOffsetYDefault.Value / characterNameContainerHeightScale) * characterNameContainerOffsetY);
			}
		}

		public static void AdjustCharacterNameContainerOffset(SoulMatesLineView __instance)
		{
			var characterNameContainer = (RectTransform)((GameObject)LineView_characterNameContainer.GetValue(__instance)).transform;

			if (!characterNameContainerOffsetYDefault.HasValue)
			{
				characterNameContainerOffsetYDefault = characterNameContainer.anchoredPosition.y;
			}

			if (characterNameContainerHeightScale != 0f)
			{
				characterNameContainer.anchoredPosition = new Vector2(
					characterNameContainer.anchoredPosition.x,
					characterNameContainerOffsetYDefault.Value + (characterNameContainerOffsetYDefault.Value / characterNameContainerHeightScale) * characterNameContainerOffsetY);
			}
		}
	}
}
