using HarmonyLib;
using SoulMates.Inventory;
using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(InventoryItem), "AttachRoomItem", new Type[] { typeof(RoomItem) })]
	public class InventoryItem_AttachRoomItem
	{
		public static float fontSizeScale;

		private static float? fontSizeDefault;

		private static FieldInfo InventoryItem_tooltipContainer;

		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					InventoryItem_tooltipContainer = typeof(InventoryItem).GetField("_tooltipContainer", BindingFlags.NonPublic | BindingFlags.Instance);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
					return false;
				}
			}

			return true;
		}

		public static void Postfix(InventoryItem __instance)
		{
			ScaleFontSize(__instance);
		}

		public static void ScaleFontSize(InventoryItem __instance)
		{
			var tooltipContainer = (Transform)InventoryItem_tooltipContainer.GetValue(__instance);
			var data = __instance.Data;

			if (!string.IsNullOrWhiteSpace((data != null) ? data.Tooltip : null))
			{
				var text = tooltipContainer.Find("Background/Text").GetComponent<TMP_Text>();

				if (!fontSizeDefault.HasValue)
				{
					fontSizeDefault = text.fontSize;
				}

				var fontSize = fontSizeDefault.Value * fontSizeScale;

				if (text.fontSizeMax < fontSize)
				{
					text.fontSizeMax = fontSize;
				}

				var oldFontSize = text.fontSize;

				text.fontSize = fontSize;

				if (text.fontSize != oldFontSize)
				{
					__instance.StopCoroutine(CoRebuildLayout(__instance));
					__instance.StartCoroutine(CoRebuildLayout(__instance));
				}
			}
		}

		private static IEnumerator CoRebuildLayout(InventoryItem __instance)
		{
			yield return null;
			yield return null;

			var tooltipContainer = (Transform)InventoryItem_tooltipContainer.GetValue(__instance);

			LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipContainer.Find("Background").GetComponent<RectTransform>());
		}
	}
}
