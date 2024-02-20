using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class light_environment : BaseEntityTable
	{
		public LightType type => model.GetComponent<Light>().type;
	}

	[RequireComponent(typeof(Light))]
	internal sealed class LightEnvironmentComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private light_environment table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
