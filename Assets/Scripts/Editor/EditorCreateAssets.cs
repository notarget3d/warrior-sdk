#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public static class EditorCreateAssets
{
	public const string FILE_EXT_MAP = ".u3d";
	public const string FILE_EXT_SOUND = ".wsd";
	public const string BUILD_DIR = "AssetBuild";


	public static bool BuildSceneAssets(string[] assets, string packName)
	{
		List<AssetBundleBuild> list = new List<AssetBundleBuild>();

		list.Add(new AssetBundleBuild()
		{
			assetBundleName = packName + FILE_EXT_MAP,
			assetNames = assets,
		});

		if (!Directory.Exists(BUILD_DIR))
		{
			Directory.CreateDirectory(BUILD_DIR);
		}

		var manifest = BuildPipeline.BuildAssetBundles(BUILD_DIR, list.ToArray(),
			BuildAssetBundleOptions.UncompressedAssetBundle |
			BuildAssetBundleOptions.AssetBundleStripUnityVersion,
			BuildTarget.StandaloneWindows64);

		return manifest != null;
	}

	public static void BuildSoundAsset()
	{
		List<AssetBundleBuild> list = new List<AssetBundleBuild>();

		List<string> assets = FindAssetsPaths<AudioClip>("Assets/sound/");
		string[] nameOverride = new string[assets.Count];

		for (int i = 0; i < nameOverride.Length; i++)
		{
			nameOverride[i] = assets[i].Substring(13);
		}

		list.Add(new AssetBundleBuild()
		{
			assetBundleName = "sounds" + FILE_EXT_SOUND,
			assetNames = assets.ToArray(),
			addressableNames = nameOverride
		});

		if (!Directory.Exists(BUILD_DIR))
		{
			Directory.CreateDirectory(BUILD_DIR);
		}

		BuildPipeline.BuildAssetBundles(BUILD_DIR, list.ToArray(),
			BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.AssetBundleStripUnityVersion,
			BuildTarget.StandaloneWindows64);
	}

	public static void BuildSoundAssetCustom(string folder, string packName, bool debugPrint = false)
	{
		List<AssetBundleBuild> list = new List<AssetBundleBuild>();

		if (folder.EndsWith('\\') == false && folder.EndsWith('/') == false)
		{
			folder += '/';
		}

		List<string> assets = FindAssetsPaths<AudioClip>(folder);
		string[] nameOverride = new string[assets.Count];

		for (int i = 0; i < nameOverride.Length; i++)
		{
			nameOverride[i] = assets[i].Substring(folder.Length);
		}

		list.Add(new AssetBundleBuild()
		{
			assetBundleName = packName + FILE_EXT_SOUND,
			assetNames = assets.ToArray(),
			addressableNames = nameOverride
		});

		if (debugPrint == true)
		{
			string msg = $"Including assets in {packName}.wsd\n";

			for (int i = 0; i < nameOverride.Length; i++)
			{
				msg += $"{nameOverride[i]}\n";
			}

			Debug.Log(msg);
		}

		AssetBuildUtils.DoBuild(list.ToArray());
	}

	public static List<string> FindAssetsPaths<T>(string path, out List<T> obj) where T : Object
	{
		List<string> assets = new List<string>();
		string type = $"t:{typeof(T).Name}";

		obj = new List<T>();

		string[] guids = AssetDatabase.FindAssets(type, new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

			if (asset != null)
			{
				assets.Add(assetPath);
				obj.Add(asset);
			}
		}

		return assets;
	}

	public static List<string> FindAssetsPaths<T>(string path) where T : Object
	{
		List<string> assets = new List<string>();
		string type = $"t:{typeof(T).Name}";

		string[] guids = AssetDatabase.FindAssets(type, new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

			if (asset != null)
			{
				assets.Add(assetPath);
			}
		}

		return assets;
	}

	public static List<T> FindAssetsByType<T>(string path) where T : Object
	{
		List<T> assets = new List<T>();
		string type = $"t:{typeof(T).Name}";

		string[] guids = AssetDatabase.FindAssets(type, new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

			if (asset != null)
			{
				assets.Add(asset);
			}
		}

		return assets;
	}
}

#endif