using UnityEngine;


namespace WMSDK.Assets
{
	[CreateAssetMenu(fileName = "Manifest", menuName = "wm/Create asset manifest", order = 3)]
	public sealed class AssetManifest : ScriptableObject
    {
        [SerializeField]
        public ScriptableObject[] assets;
    }
}
