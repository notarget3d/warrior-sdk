#if UNITY_EDITOR

using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using WMSDK.Assets;

using Object = UnityEngine.Object;

namespace WMSDK
{
    public static class AssetBuildScript
    {
		public const string TEXTURES_TEMP_MANIFEST = "textures.txt";
		public const string TEXTURES_SHARED_MANIFEST = "textures_shared.txt";
		public const string BUILD_DIR = "RuntimeWarrioAssets";
        public const string FILE_PACK_EXT = ".war";
		public const string FILE_EXT_MAP = ".u3d";
		public const string BUNDLE_PATH_REMOVE = "Assets/";
		public const string SCENE_TEXTURES_MANIFEST_PATH = BUILD_DIR + "/" + TEXTURES_TEMP_MANIFEST;
		public const string SCENE_TEXTURES_MANIFEST_SHARED_PATH = BUILD_DIR + "/" + TEXTURES_SHARED_MANIFEST;


		public static AssetBundleManifest DoBuild(AssetBundleBuild[] list)
        {
            EnsureBuildDir();

            return BuildPipeline.BuildAssetBundles(BUILD_DIR, list,
                BuildAssetBundleOptions.UncompressedAssetBundle |
                BuildAssetBundleOptions.AssetBundleStripUnityVersion |
				BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.StandaloneWindows64);
        }

        public static string GetAssetPath(Object assetObject)
        {
            return AssetDatabase.GetAssetPath(assetObject);
        }

        public static void BuildModelsFromManifest(AssetManifest manifest)
        {
            List<string> assetsPath = new List<string>(256);
            List<MaterialsManifest.Description> warMatList = new List<MaterialsManifest.Description>(512);
            HashSet<string> materials = new HashSet<string>(512);
            HashSet<string> textures = new HashSet<string>(1024);

			MaterialsManifest materialsManifest = MaterialsManifest.CreateInstance<MaterialsManifest>();

			for (int i = 0; i < manifest.assets.Length; i++)
            {
                GenericManifest generic = manifest.assets[i] as GenericManifest;

                if (generic is null)
                {
                    continue;
                }

                for (int f = 0; f < generic.objects.Length; f++)
                {
                    Object obj = generic.objects[f];

                    if (obj is GameObject)
                    {
                        string prefabPath = GetAssetPath(obj);

                        GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);

                        assetsPath.Add(prefabPath);
                        List<MaterialsManifest.Description> newMaterials =
							ProcessAssetRenderers(prefab, warMatList, manifest.packName);

                        GetAssetUnityMaterialPaths(prefab, materials);

                        for (int j = 0; j < newMaterials.Count; j++)
                        {
                            newMaterials[j].GetTextureList(textures);
                        }

                        EditorUtility.SetDirty(prefab);
                        PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);

                        PrefabUtility.UnloadPrefabContents(prefab);
                    }
                }
            }

			string manifestPath = "Assets/__materials_manifest.asset";
			materialsManifest.materials = warMatList.ToArray();
			if (AssetDatabase.AssetPathExists(manifestPath))
			{
				AssetDatabase.DeleteAsset(manifestPath);
			}

			AssetDatabase.CreateAsset(materialsManifest, manifestPath);
			AssetDatabase.SaveAssets();
			assetsPath.Add(manifestPath);

			EnsureBuildDir();
            PrintAssets(assetsPath);

            string mainPackFileName = manifest.packName + FILE_PACK_EXT;
            string texturePackName = manifest.packName + "_tex" + FILE_PACK_EXT;

            AssetBundleBuild mainPack = MakeManifestGroup(mainPackFileName, assetsPath.ToArray());
            AssetBundleBuild tempPack = MakeManifestGroup("_tmp", materials.ToArray());
            AssetBundleBuild texPack = MakeManifestGroup(texturePackName, textures.ToArray());

            DoBuild(new AssetBundleBuild[] { mainPack, tempPack });
            DoBuild(new AssetBundleBuild[] { texPack });

            ClearAssetsMaterialDescription(manifest);
        }

        public static void BuildTexturesFromManifest(AssetManifest manifest)
        {
            List<string> assetsPath = new List<string>(256);

            for (int i = 0; i < manifest.assets.Length; i++)
            {
                GenericManifest generic = manifest.assets[i] as GenericManifest;

                if (generic is null)
                {
                    continue;
                }

                for (int f = 0; f < generic.objects.Length; f++)
                {
                    Object obj = generic.objects[f];

                    // Include GenericManifest for sprites
                    if (obj is Texture2D || obj is GenericManifest)
                    {
                        string texturePath = GetAssetPath(obj);
                        assetsPath.Add(texturePath);
                    }
                }
            }

            EnsureBuildDir();
            PrintAssets(assetsPath);

            string mainPackFileName = manifest.packName + FILE_PACK_EXT;

            AssetBundleBuild mainPack = MakeManifestGroup(mainPackFileName, assetsPath.ToArray());

            DoBuild(new AssetBundleBuild[] { mainPack });
        }

		public static List<MaterialsManifest.Description> ProcessDuplicates(MaterialsManifest.Description[] description,
			List<MaterialsManifest.Description> existingMaterials)
		{
			List<MaterialsManifest.Description> newMatList = new List<MaterialsManifest.Description>(8);

			for (int f = 0; f < description.Length; f++)
			{
				MaterialsManifest.Description info = description[f];

				int idx = existingMaterials.FindIndex(x => x.Equals(info));

				if (idx == -1)
				{
					newMatList.Add(info);
					existingMaterials.Add(info);
				}
				else
				{
					description[f] = existingMaterials[idx];
				}
			}

			return newMatList;
		}

		public static bool BuildSceneAssets(string[] assets, string packName)
		{
			List<AssetBundleBuild> list = new List<AssetBundleBuild>();

			list.Add(new AssetBundleBuild()
			{
				assetBundleName = packName + FILE_EXT_MAP,
				assetNames = assets,
			});

			return DoBuild(list.ToArray()) != null;
		}

		public static void BuildSceneTexturesAssets(string packName, string manifestPath, bool removeTempManifest)
		{
			if (!File.Exists(manifestPath))
			{
				Debug.LogError($"Unable to get texture manifest! File does not exists '{manifestPath}'");
				return;
			}

			string[] lines = File.ReadAllLines(manifestPath);

			AssetBundleBuild list = MakeManifestGroup(packName + FILE_PACK_EXT, lines);

			DoBuild(new AssetBundleBuild[] { list });

			if (removeTempManifest)
			{
				File.Delete(manifestPath);
			}
		}

		private static AssetBundleBuild MakeManifestGroup(string fileName, string[] path)
        {
            string[] pathAddress = new string[path.Length];

            for (int i = 0; i < pathAddress.Length; i++)
            {
                pathAddress[i] = path[i].Remove(0, BUNDLE_PATH_REMOVE.Length);
            }

            return new AssetBundleBuild()
            {
                assetBundleName = fileName,
                assetNames = path,
                addressableNames = pathAddress
            };
        }

		public static string[] GetMaterialIds(MaterialsManifest.Description[] materials)
		{
			string[] result = new string[materials.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = materials[i].id;
			}

			return result;
		}

		private static void PrintAssets(List<string> assetList)
        {
            StringBuilder sb = new StringBuilder(1024);

            for (int i = 0; i < assetList.Count; i++)
            {
                sb.AppendLine(assetList[i]);
            }

            Debug.Log(sb.ToString());
        }

        private static void EnsureBuildDir()
        {
            if (!Directory.Exists(BUILD_DIR))
            {
                Directory.CreateDirectory(BUILD_DIR);
            }
        }

        private static void GetAssetUnityMaterialPaths(GameObject gameObject, HashSet<string> result)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);

            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] materials = renderers[i].sharedMaterials;

                foreach (Material mat in materials)
                {
                    if (mat == null)
                    {
                        continue;
                    }

                    string path = GetAssetPath(mat);

                    if (string.IsNullOrEmpty(path))
                    {
                        continue;
                    }

                    result.Add(path);
                }
            }
        }

		private static List<MaterialsManifest.Description> ProcessAssetRenderers(GameObject gameObject,
			List<MaterialsManifest.Description> warMatList, string source)
        {
			List<MaterialsManifest.Description> newMatList = new List<MaterialsManifest.Description>(8);
			Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
				MaterialsManifest.Description[] description = MaterialsManifest.Create(renderer, source);

                if (!renderer.gameObject.TryGetComponent(out MaterialsReference warriorMaterial))
                {
                    warriorMaterial = renderer.gameObject.AddComponent<MaterialsReference>();
                }

				newMatList.AddRange(ProcessDuplicates(description, warMatList));
				warriorMaterial.materialIds = GetMaterialIds(description);
            }

            return newMatList;
        }

        private static void ClearAssetsMaterialDescription(AssetManifest manifest)
        {
            for (int i = 0; i < manifest.assets.Length; i++)
            {
                GenericManifest generic = manifest.assets[i] as GenericManifest;

                if (generic is null)
                {
                    continue;
                }

                for (int f = 0; f < generic.objects.Length; f++)
                {
                    Object obj = generic.objects[f];

                    if (obj is GameObject)
                    {
                        string prefabPath = GetAssetPath(obj);

                        GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);

                        MaterialsReference[] info = prefab.GetComponentsInChildren<MaterialsReference>(true);

                        for (int j = 0; j < info.Length; j++)
                        {
                            Component.DestroyImmediate(info[j], true);
                        }

                        EditorUtility.SetDirty(prefab);
                        PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);

                        PrefabUtility.UnloadPrefabContents(prefab);
                    }
                }
            }
        }
    }
}

#endif