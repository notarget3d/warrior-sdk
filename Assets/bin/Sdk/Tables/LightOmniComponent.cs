using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class light_omni : BaseEntityTable
	{
		public LightType type => model.GetComponent<Light>().type;
	}

	[RequireComponent(typeof(Light))]
	[AddComponentMenu("Entities/" + nameof(light_omni))]
	internal sealed class LightOmniComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private light_omni table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
