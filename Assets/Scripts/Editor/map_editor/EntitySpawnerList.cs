public enum EntitySpawnerListType
{
	WEAPONS = 1 << 0,
	ITEMS = 1 << 1,
	NPCS = 1 << 2
}

public static class EntitySpawnerList
{
	public static string[] WEAPONS => new string[]
	{
		"null",
		"weapon_pistol",
		"weapon_deagle",
		"weapon_smg_1",
		"weapon_pumpshotgun",
		"weapon_rifle_m4",
		"weapon_rifle_ak74m",
	};

	public static string[] ITEMS => new string[]
	{
		"null",
		"item_bonus_health",
		"item_bonus_mega",
	};

	public static string[] NPCS => new string[]
	{
		"null",
		"npc_zombie",
		"npc_thug",
	};
}
