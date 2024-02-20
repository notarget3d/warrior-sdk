using UnityEngine;


namespace WMSDK.Assets
{
	public enum WeaponAnimType : byte
	{
		RIFLE, PISTOL, SHOTGUN, SMG, HEAVY, SHOULDER
	}


	public sealed class WeaponModelDescription : MonoBehaviour
    {
		public WeaponAnimType type;
		public GameObject mesh;
		public Transform rhand;
		public Transform lhand;
		public Transform muzzle;
		public Transform shell;
	}
}
