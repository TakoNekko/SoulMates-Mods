using HarmonyLib;
using JuicyPanda;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using SoulMatesLineView = SoulMates.Dialogue.LineView;

namespace BepInEx5Plugins.Ash.SoulMates.TextScaling.HarmonyPatches
{
	[HarmonyPatch(typeof(SoulMatesLineView), "UserRequestedViewAdvancement")]
	public class LineView_UserRequestedViewAdvancement
	{
		public static float arrowOffsetY;

		private static Vector3? arrowGoalDefault;

		private static FieldInfo LineView_lineText;
		private static FieldInfo LineView_visible;
		private static FieldInfo LineView_Arrow;
		private static FieldInfo SpringPosition_goal;

		public static bool Prepare(MethodBase original)
		{
			if (original is null)
			{
				try
				{
					LineView_lineText = typeof(SoulMatesLineView).GetField("_lineText", BindingFlags.NonPublic | BindingFlags.Instance);
					LineView_visible = typeof(SoulMatesLineView).GetField("_visible", BindingFlags.NonPublic | BindingFlags.Instance);
					LineView_Arrow = typeof(SoulMatesLineView).GetField("_Arrow", BindingFlags.NonPublic | BindingFlags.Instance);
					SpringPosition_goal = typeof(SpringPosition).GetField("_goal", BindingFlags.NonPublic | BindingFlags.Instance);
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
			AdjustArrowSpringPositionGoal(__instance);
		}

		public static void AdjustArrowSpringPositionGoal(SoulMatesLineView __instance)
		{
			var lineText = (TMP_Juicy)LineView_lineText.GetValue(__instance);
			var visible = (bool)LineView_visible.GetValue(__instance);

			if (!visible || lineText.Typing)
			{
				return;
			}

			var arrow = (Image)LineView_Arrow.GetValue(__instance);
			var springPosition = arrow.GetComponent<SpringPosition>();

			if (!arrowGoalDefault.HasValue)
			{
				arrowGoalDefault = (Vector3)SpringPosition_goal.GetValue(springPosition);
			}

			if (LineView_RunLine.containerHeightScale != 0f)
			{
				springPosition.SetGoal(new Vector3(arrowGoalDefault.Value.x, arrowGoalDefault.Value.y + (arrowGoalDefault.Value.y / LineView_RunLine.containerHeightScale) * arrowOffsetY, arrowGoalDefault.Value.z));
			}
		}
	}
}
