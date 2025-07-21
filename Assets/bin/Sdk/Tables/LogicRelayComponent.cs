using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class logic_relay : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/" + nameof(logic_relay))]
	internal sealed class LogicRelayComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private logic_relay table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(logic_relay));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
