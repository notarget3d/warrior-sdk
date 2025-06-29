using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class env_soundscape : BaseEntityTable
	{
		[Min(0.01f)] public float radius = 5.0f;
        public string soundscape;
        public Transform[] soundPosition;
    }

	[AddComponentMenu("Entities/" + nameof(env_soundscape))]
	internal sealed class EnvSoundscapeComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private env_soundscape table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(env_soundscape_proxy));
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 origin = transform.position;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(origin, table.radius);

			DrawDefaultGizmoSelectedWire();

			if (table.soundPosition == null)
			{
				return;
			}

			Gizmos.color = Color.yellow;

			for (int i = 0; i < table.soundPosition.Length; i++)
			{
				if (table.soundPosition[i] == null)
				{
					continue;
				}

				Gizmos.DrawLine(origin, table.soundPosition[i].position);
				Gizmos.DrawWireSphere(table.soundPosition[i].position, 0.1f);
			}
		}
	}
}
