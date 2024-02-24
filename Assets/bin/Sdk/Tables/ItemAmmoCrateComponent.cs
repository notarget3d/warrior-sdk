using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class item_ammo_crate : BaseEntityTable
	{
		[DropdownDrawer("None", "Pistol", "Magnum", "SMG", "Rifle", "Sniper", "Shotgun")]
		public int ammoType = 1;
		public bool useCustomModel;
	}

	internal sealed class ItemAmmoCrateComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private item_ammo_crate table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix; 
			Gizmos.DrawWireCube(new Vector3(0.0f, 0.35f, 0.0f), new Vector3(0.6f, 0.7f, 1.2f));

			Gizmos.color = Color.red;
			Gizmos.DrawLine(Vector3.zero, new Vector3(0.35f, 0.0f, 0.0f));
		}
	}
}