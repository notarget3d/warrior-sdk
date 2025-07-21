public static class EntityIOList
{
	public static readonly string[] OUTPUTS = new string[]
	{
		"Null",
		"OnFireUser1",
		"OnFireUser2",
		"OnFireUser3",
		"OnFireUser4",
		"OnDamaged",
		"OnBreak",
		// button
		"OnPressed",
		"OnUseLocked",
		"OnIn",
		"OnOut",
		// relay, trigger
		"OnTrigger",
		"OnStartTouch",
		"OnStartTouchAll",
		"OnEndTouch",
		"OnEndTouchAll",
		"OnTouching",
		"OnNotTouching",
		// counter
		"OutValue",
		"OnHitMin",
		"OnHitMax",
		"OnGetValue",
		"OnChangedFromMin",
		"OnChangedFromMax",
		// relay
		"OnSpawn",
		// branch
		"OnTrue",
		"OnFalse",
		// auto
		"OnMapSpawn",
		"OnNewGame",
		"OnLoadGame",
		"OnMapTransition"
	};

	public static readonly string[] INPUTS = new string[]
	{
		"None",
		"Disable",
		"Enable",
		"Lock",
		"Unlock",
		"Kill",
		"Toggle",
		"AddOutput",
		"SetColor",
		"TurnOn",
		"TurnOff",
		"TogglePlay",
		"Play",
		"Stop",
		"SetSound",
		"SetVolume",
		"TouchTest",
		"SetDestIdx",
		"SetPushForce",
		"Press",
		"PressIn",
		"PressOut",
		"Break",
		"ForceSpawn",
		"NpcSpawn",
		"GoToFloor",
		"ToggleElevator",
		"StopElevator",
		"StopConveyor",
		"StartConveyor",
		"Close",
		"Open",
		"ToggleDoor",
		"ShowMessage",
		"ChangeLevel",
		"Use",
		"TriggerGameEnd",
		"RollCredits",
		"SurvivalStart",

		// branch, counter
		"SetValue",

		// relay
		"Trigger",
		// branch
		"SetValueTest",
		"ToggleState",
		"ToggleStateTest",
		"Test",
		// counter
		"Add",
		"Divide",
		"Multiply",
		"SetValueNoFire",
		"Subtract",
		"SetHitMax",
		"SetHitMin",
		"GetValue",
		"SetMaxValueNoFire",
		"SetMinValueNoFire",
	};
}
