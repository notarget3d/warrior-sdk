using System;


namespace WMSDK
{
	[Serializable]
	public abstract class BaseNpcSoldierTable : BaseEntityTable
	{
		[DropdownStringDrawer(EntitySpawnerListType.WEAPONS)]
		public string spawnWeapon;
	}
}