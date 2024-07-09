using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class game_message : BaseEntityTable
	{
		[TextArea(3, 5)]
		public string message;

		[DropdownDrawer("DEBUG", "CHAT_ALL", "CHAT_TEAM", "MSG_WHITE",
			"MSG_RED", "MSG_GREEN", "MSG_BLUE", "MSG_YELLOW")]
		public int msgType = 1;
	}

	[AddComponentMenu("Entities/" + nameof(game_message))]
	internal sealed class GameMessageComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private game_message table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(game_message));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
