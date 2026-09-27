using UnityEngine;


namespace WMSDK.Assets
{
	[CreateAssetMenu(fileName = "Manifest", menuName = "wm/Create asset manifest", order = 3)]
	public sealed class AssetManifest : ScriptableObject
    {
        public enum ManifestBuildType
        {
            Prefabs, Textures
        }

        [SerializeField]
        public ManifestBuildType type;

        [SerializeField]
        public string packName;

        [SerializeField]
        public ScriptableObject[] assets;

#if UNITY_EDITOR
        [ContextMenu("Build")]
        public void Build()
        {
            switch (type)
            {
                case ManifestBuildType.Prefabs:
                    AssetBuildScript.BuildModelsFromManifest(this);
                    break;
                case ManifestBuildType.Textures:
                    AssetBuildScript.BuildTexturesFromManifest(this);
                    break;
            }
        }
#endif
    }
}
