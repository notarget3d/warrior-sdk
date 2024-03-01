using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_ladder : BaseEntityTable
	{
		public Transform ladderBotPt;
		public Transform ladderTopPt;
		public Transform[] ladderDismount;
	}

	internal sealed class FuncLadderComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_ladder table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			var s1 = table.ladderBotPt;
			var s2 = table.ladderTopPt;

			if (s1 != null && s2 != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(s1.position, s2.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(s1.position, s1.position + GetLadderNormal());

				DrawPlayerGizmo(s1.position, Color.yellow);
				DrawPlayerGizmo(s2.position, Color.yellow);

				for (int i = 0; i < table.ladderDismount.Length; i++)
				{
					DrawPlayerGizmo(table.ladderDismount[i].position, Color.yellow);
				}
			}
		}

		private Vector3 GetLadderNormal()
		{
			var s1 = table.ladderBotPt;
			var s2 = table.ladderTopPt;

			return Vector3.Cross(s1.right, s1.position - s2.position).normalized;
		}

		private static void DrawPlayerGizmo(Vector3 pos, Color color) =>
			Utils.DrawPlayerGizmo(pos, color);
	}
}
