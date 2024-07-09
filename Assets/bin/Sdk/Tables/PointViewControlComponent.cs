using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class point_viewcontrol : BaseEntityTable
	{
		public string target;
		public float wait;
		public bool interpolate;
	}

	[AddComponentMenu("Entities/" + nameof(point_viewcontrol))]
	internal sealed class PointViewControlComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private point_viewcontrol table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo();
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelected();
		}
	}
}
