using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class env_soundscape_proxy : BaseEntityTable
	{
        public string soundscapeReference;
		[Min(0.01f)] public float radius = 5.0f;
    }

	[AddComponentMenu("Entities/" + nameof(env_soundscape_proxy))]
	internal sealed class EnvSoundscapeProxyComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private env_soundscape_proxy table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(env_soundscape_proxy));
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, table.radius);

			DrawDefaultGizmoSelectedWire();
		}
	}
}
