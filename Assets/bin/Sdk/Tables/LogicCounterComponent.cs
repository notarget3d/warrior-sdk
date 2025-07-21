using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class logic_counter : BaseEntityTable
	{
		public float initialValue;
		public float minValue;
		public float maxValue = 10.0f;
	}

	[AddComponentMenu("Entities/" + nameof(logic_counter))]
	internal sealed class LogicCounterComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private logic_counter table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(logic_counter));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
