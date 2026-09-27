using System;
using UnityEngine;

namespace WMSDK.Assets
{
    public sealed class MaterialDescriptionScene : MonoBehaviour
    {
		[Serializable]
		public sealed class EntryKeyValue
		{
			public string key;
			public MaterialsManifest.Description description;

			public EntryKeyValue(string _key, MaterialsManifest.Description _description)
			{
				key = _key;
				description = _description;
			}
		}

		[Serializable]
		public sealed class EntryRenderer
		{
			public Renderer renderer;
			public string[] descriptionIds;

			public EntryRenderer(Renderer _renderer, string[] _descriptionIds)
			{
				renderer = _renderer;
				descriptionIds = _descriptionIds;
			}
		}

		[SerializeField]
		public EntryRenderer[] RendererEntries;

		[SerializeField]
		public EntryKeyValue[] MaterialEntries;
	}
}
