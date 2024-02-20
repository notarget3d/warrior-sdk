using UnityEngine;


namespace WMSDK.Assets
{
	[CreateAssetMenu(fileName = "GenericManifest", menuName = "wm/Create generic asset list", order = 4)]
	public sealed class GenericManifest : ScriptableObject
    {
        [SerializeField]
        public Object[] objects;
    }
}
