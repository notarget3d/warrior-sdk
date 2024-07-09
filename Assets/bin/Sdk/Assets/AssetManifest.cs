using UnityEngine;


namespace WMSDK.Assets
{
	[CreateAssetMenu(fileName = "Manifest", menuName = "wm/Create asset manifest", order = 3)]
	public sealed class AssetManifest : ScriptableObject
    {
        [SerializeField]
        public ScriptableObject[] assets;

#if UNITY_EDITOR

        public string fileName;

        [ContextMenu("Build Asset")]
        public void BuildAsset()
        {
			string path = UnityEditor.AssetDatabase.GetAssetPath((Object)this);
			UnityEditor.AssetBundleBuild bundle = new UnityEditor.AssetBundleBuild();
			bundle.assetBundleName = fileName;
			bundle.assetNames = new string[] { path };

			AssetBuildUtils.DoBuild(new[] { bundle } );
		}
#endif
    }
}
