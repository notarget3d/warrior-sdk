using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class logic_auto : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/" + nameof(logic_auto))]
	internal sealed class LogicAutoComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private logic_auto table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(logic_auto));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
