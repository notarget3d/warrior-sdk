#if UNITY_EDITOR
using System;


public partial class EditorGenericSpawnFlagsDrawer
{
	private const int START_DISABLED = 1 << 0;
	private const int START_LOCKED = 1 << 1;
	private const int IGNORE_AIM_ASSIST = 1 << 2;
	private const int BASE = 1 << 15;

	[Flags]
	public enum generic_entity
	{
		StartDisabled = START_DISABLED,
		StartLocked = START_LOCKED,
		IgnoreAimAssist = IGNORE_AIM_ASSIST,
	}

	[Flags]
	public enum func_breakable
	{
		StartDisabled = START_DISABLED,
		BreakableByPlayers = BASE << 1,
		NetworkDamageEvents = BASE << 2
	}

	[Flags]
	public enum point_item_spawner
	{
		StartDisabled = START_DISABLED,
		SpawnOnceOnly = BASE << 1,
		SpawnOnStart = BASE << 2
	}

	[Flags]
	public enum npc_maker
	{
		StartDisabled = START_DISABLED,
		SpawnOnStart = BASE << 2
	}

	[Flags]
	public enum func_door
	{
		StartDisabled = START_DISABLED,
		StartLocked = START_LOCKED,
		Toggle = BASE << 1,
		UseOpens = BASE << 2,
		BlockOpens = BASE << 3
	}

	[Flags]
	public enum ambient_generic
	{
		StartDisabled = START_DISABLED,
		PlayOnStart = BASE << 1,
		Looped = BASE << 2,
		PlayEverywhere = BASE << 3,
		unused = BASE << 4
	}

	[Flags]
	public enum trigger_push
	{
		StartDisabled = START_DISABLED,
		PushPhysics = BASE << 1,
	}

	[Flags]
	public enum func_conveyor
	{
		StartDisabled = START_DISABLED,
		StartActive = BASE << 1,
		WorldSpace = BASE << 2
	}

	[Flags]
	public enum npc_thug
	{
		StartDisabled = START_DISABLED,
		StartPatroling = BASE << 1
	}
}

#endif