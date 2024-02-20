using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class ambient_generic : BaseEntityTable
	{
		public float distMin;
		public float distMax;
		public float volume;
		public string sound;
		public string channel;
	}

	internal sealed class AmbientGenericComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private ambient_generic table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
