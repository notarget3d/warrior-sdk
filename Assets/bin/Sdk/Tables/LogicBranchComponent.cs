using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class logic_branch : BaseEntityTable
	{
		public bool initialValue;
	}

	[AddComponentMenu("Entities/" + nameof(logic_branch))]
	internal sealed class LogicBranchComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private logic_branch table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(logic_branch));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
