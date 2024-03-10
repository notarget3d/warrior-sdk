using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class light_spot : BaseEntityTable
	{
		public LightType type => model.GetComponent<Light>().type;
	}

	[RequireComponent(typeof(Light))]
	[AddComponentMenu("Entities/light_spot")]
	internal sealed class LightSpotComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private light_spot table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
