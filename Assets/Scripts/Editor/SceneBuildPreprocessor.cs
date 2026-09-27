#if UNITY_EDITOR

using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using WMSDK.Assets;
using WMSDK;


public sealed class SceneBuildPreprocessor : IProcessSceneWithReport
{
	int IOrderedCallback.callbackOrder => 1;

	void IProcessSceneWithReport.OnProcessScene(Scene scene, BuildReport report)
	{
		if (!BuildPipeline.isBuildingPlayer)
		{
			return;
		}

		EntityTableUtils.UpdateAllEntityTables();

		Debug.Log($"Processing scene: {scene.name}");
		GameObject materialsManifest = new GameObject("materials_manifest");
		MaterialDescriptionScene manifestComponent = materialsManifest.AddComponent<MaterialDescriptionScene>();
		List<MaterialDescriptionScene.EntryRenderer> entries = new List<MaterialDescriptionScene.EntryRenderer>(1024);
		List<MaterialDescriptionScene.EntryKeyValue> materialsDescription = new List<MaterialDescriptionScene.EntryKeyValue>(256);
		List<MaterialsManifest.Description> dupList = new List<MaterialsManifest.Description>(1024);
		HashSet<string> textureList = new HashSet<string>(128);
		List<string> renderersList = new List<string>(1024);

		Renderer[] renderers = GameObject.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

		for (int i = 0; i < renderers.Length; i++)
		{
			Renderer renderer = renderers[i];
			renderersList.Add(renderer.gameObject.name);
			MaterialsManifest.Description[] description = MaterialsManifest.Create(renderer, scene.name);
			List<MaterialsManifest.Description> newMaterials = AssetBuildScript.ProcessDuplicates(description, dupList);
			entries.Add(new MaterialDescriptionScene.EntryRenderer(renderer, AssetBuildScript.GetMaterialIds(description)));

			Material[] materials = renderer.sharedMaterials;

			for (int f = 0; f < materials.Length; f++)
			{
				materials[f] = null;
			}

			for (int f = 0; f < newMaterials.Count; f++)
			{
				materialsDescription.Add(new MaterialDescriptionScene.EntryKeyValue(newMaterials[f].id, newMaterials[f]));
				newMaterials[f].GetTextureList(textureList);
			}

			renderer.sharedMaterials = materials;
		}

		manifestComponent.RendererEntries = entries.ToArray();
		manifestComponent.MaterialEntries = materialsDescription.ToArray();

		HandleTextureManifest(AssetBuildScript.SCENE_TEXTURES_MANIFEST_PATH, new HashSet<string>(textureList));
		HandleTextureManifest(AssetBuildScript.SCENE_TEXTURES_MANIFEST_SHARED_PATH, textureList);
	}

	private static void HandleTextureManifest(string manifestPath, HashSet<string> textureList)
	{
		if (File.Exists(manifestPath))
		{
			string[] lines = File.ReadAllLines(manifestPath);

			for (int i = 0; i < lines.Length; i++)
			{
				textureList.Add(lines[i]);
			}
		}

		string textures = MakeTextureList(textureList);
		File.WriteAllText(manifestPath, textures);
	}

	private static string MakeTextureList(HashSet<string> textureList)
	{
		StringBuilder sb = new StringBuilder(1024 * 64);

		foreach (var texture in textureList)
		{
			sb.AppendLine(texture);
		}

		return sb.ToString();
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
}

#endif