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
	internal sealed class LightOmniComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private light_omni table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
