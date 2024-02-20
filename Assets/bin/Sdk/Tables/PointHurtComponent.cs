using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class point_hurt : BaseEntityTable
	{
		public float radius = 1.0f;
		public float delay = 1.0f;

		public short damageAmount;
		public int damageType;
	}

	internal sealed class PointHurtComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private point_hurt table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
