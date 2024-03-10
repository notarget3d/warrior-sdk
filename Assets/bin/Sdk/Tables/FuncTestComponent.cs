using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_test : BaseEntityTable
	{
		public Vector3 rotAxis = Vector3.up;
		public float speed = 500.0f;
		public float offset = 1.0f;
	}

	[AddComponentMenu("Entities/func_test")]
	internal sealed class FuncTestComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_test table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
