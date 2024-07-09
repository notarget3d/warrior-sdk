#if UNITY_EDITOR

using System.IO;
using UnityEditor;


public static class AssetBuildUtils
{
	public static string BUILD_DIR => EditorCreateAssets.BUILD_DIR;

	public static void DoBuild(AssetBundleBuild[] list)
	{
		if (!Directory.Exists(BUILD_DIR))
		{
			Directory.CreateDirectory(BUILD_DIR);
		}

		BuildPipeline.BuildAssetBundles(BUILD_DIR, list,
			BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.AssetBundleStripUnityVersion,
			BuildTarget.StandaloneWindows64);
	}

	public static void BuildAssetManifest(WMSDK.Assets.AssetManifest[] manifests)
	{
		AssetBundleBuild[] bundles = new AssetBundleBuild[manifests.Length];

		for (int i = 0; i < manifests.Length; i++)
		{
			string path = AssetDatabase.GetAssetPath(manifests[i]);
			string fileName = manifests[i].fileName;

			AssetBundleBuild bundle = new AssetBundleBuild();
			bundle.assetBundleName = fileName;
			bundle.assetNames = new string[] { path };

			bundles[i] = bundle;
		}

		DoBuild(bundles);
	}

	public static void BuildAssetManifest(WMSDK.Assets.AssetManifest manifest)
	{
		BuildAssetManifest(manifest, manifest.fileName);
	}

	public static void BuildAssetManifest(WMSDK.Assets.AssetManifest manifest, string fileName)
	{
		string path = AssetDatabase.GetAssetPath(manifest);
		AssetBundleBuild bundle = new AssetBundleBuild();
		bundle.assetBundleName = fileName;
		bundle.assetNames = new string[] { path };

		DoBuild(new[] { bundle });
	}
}

#endif