using HarmonyLib;
using System;
using UnityEngine;
using SoulMatesCursor = SoulMates.Cursors.Cursor;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(SoulMatesCursor), "DrawTooltip", new Type[] { typeof(Vector2), typeof(string) })]
	public class Cursor_DrawTooltip
	{
		public static float fontSizeScale;

		private static float? fontSizeDefault;

		public static bool Prefix(SoulMatesCursor __instance, Vector2 position, string tooltip)
		{
			var content = new GUIContent(tooltip);
			var style = new GUIStyle(GUI.skin.label);

			if (!fontSizeDefault.HasValue)
			{
				fontSizeDefault = 14;
			}

			style.fontSize = Mathf.CeilToInt(fontSizeDefault.Value * fontSizeScale);

			var size = style.CalcSize(content);
			var rectSize = new Vector2(20f * size.x, 20f * size.y);
			
			GUI.Label(new Rect(position, rectSize), content, style);

			return false;
		}
	}
}
