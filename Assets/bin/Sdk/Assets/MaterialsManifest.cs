using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace WMSDK.Assets
{
    public sealed class MaterialsManifest : ScriptableObject
    {
		public const string SHADER_DYN_OPAQUE = "DynamicLit";
		public const string SHADER_DYN_TRANSPARENT = "DynamicLitTransparent";
		public const string SHADER_PARTICLE = "ParticleLit";
		public const string SHADER_PARTICLE_UNLIT = "ParticleUnlit";
		public const string SHADER_PARTICLE_CLOUD = "ParticleCloud";
		public const string SHADER_WATER = "WarriorWater";
		public const string SHADER_STATIC = "StaticLit";
		public const string SHADER_ERROR = "Error";

		public static int PROP_BASE_COLOR = Shader.PropertyToID("_BaseColor");
		public static int PROP_ALBEDO = Shader.PropertyToID("_BaseMap");
		public static int PROP_NORMAL = Shader.PropertyToID("_BumpMap");
		public static int PROP_EMISSION = Shader.PropertyToID("_EmissionMap");
		public static int PROP_METALLIC = Shader.PropertyToID("_MetallicGlossMap");
		public static int PROP_SPECULAR = Shader.PropertyToID("_SpecGlossMap");
		public static int PROP_EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");

		public static int PROP_WATER_NORMAL_A = Shader.PropertyToID("_NormalMapA");
		public static int PROP_WATER_NORMAL_B = Shader.PropertyToID("_NormalMapB");

		private const string BASE_PATH_REMOVE = "Assets/";

		[SerializeField]
		public Description[] materials;

		[Serializable]
		public sealed class PropKeyValueFloat
		{
			public string key;
			public float value;

			public PropKeyValueFloat(string k, float v)
			{
				key = k;
				value = v;
			}

			public bool Equals(PropKeyValueFloat other)
			{
				if (key != other.key)
				{
					return false;
				}

				if (value != other.value)
				{
					return false;
				}

				return true;
			}
		}

		[Serializable]
		public sealed class PropKeyValueVector
		{
			public string key;
			public Vector4 value;

			public PropKeyValueVector(string k, Vector4 v)
			{
				key = k;
				value = v;
			}

			public bool Equals(PropKeyValueVector other)
			{
				if (key != other.key)
				{
					return false;
				}

				if (value != other.value)
				{
					return false;
				}

				return true;
			}
		}

		[Serializable]
		public sealed class Description
		{
			public const string INVALID_MATERIAL = "ERROR";

			public string id;
			public string shader;
			public string unityShader;

			public string albedo;
			public string normal;
			public string emission;
			public string metal;
			public string specular;
			public int renderQueue;

			public PropKeyValueFloat[] propFloats;
			public PropKeyValueVector[] propVectors;

			public string[] keywords;


			public Description()
			{
				id = INVALID_MATERIAL;
				shader = SHADER_ERROR;
				unityShader = string.Empty;
				albedo = string.Empty;
				normal = string.Empty;
				emission = string.Empty;
				metal = string.Empty;
				specular = string.Empty;
				renderQueue = 2000;
				propFloats = Array.Empty<PropKeyValueFloat>();
				propVectors = Array.Empty<PropKeyValueVector>();
				keywords = Array.Empty<string>();
			}

			public Description(string _id, string _shader, string _unityShader)
			{
				id = _id;
				shader = _shader;
				unityShader = _unityShader;
				albedo = string.Empty;
				normal = string.Empty;
				emission = string.Empty;
				metal = string.Empty;
				specular = string.Empty;
				renderQueue = 2000;
				propFloats = Array.Empty<PropKeyValueFloat>();
				propVectors = Array.Empty<PropKeyValueVector>();
				keywords = Array.Empty<string>();
			}

			public Description(string _id, string _shader, string _unityShader, string _albedo, string _normal, string _emission,
				string _metal, string _specular, int _renderQueue,
				PropKeyValueFloat[] _propFloats, PropKeyValueVector[] _propVectors, string[] _keywords)
			{
				id = _id;
				shader = _shader;
				unityShader = _unityShader;
				albedo = _albedo;
				normal = _normal;
				emission = _emission;
				metal = _metal;
				renderQueue = _renderQueue;
				specular = _specular;
				propFloats = _propFloats;
				propVectors = _propVectors;
				keywords = _keywords;
			}

			public Description(string _id, string _shader, string _unityShader, string _albedo, int _renderQueue,
				PropKeyValueFloat[] _propFloats, PropKeyValueVector[] _propVectors, string[] _keywords)
			{
				id = _id;
				shader = _shader;
				unityShader = _unityShader;
				albedo = _albedo;
				normal = string.Empty;
				emission = string.Empty;
				metal = string.Empty;
				specular = string.Empty;
				renderQueue = _renderQueue;
				propFloats = _propFloats;
				propVectors = _propVectors;
				keywords = _keywords;
			}

			public bool Equals(Description other)
			{
				if (shader != other.shader)
				{
					return false;
				}

				if (unityShader != other.unityShader)
				{
					return false;
				}

				if (albedo != other.albedo)
				{
					return false;
				}

				if (normal != other.normal)
				{
					return false;
				}

				if (emission != other.emission)
				{
					return false;
				}

				if (metal != other.metal)
				{
					return false;
				}

				if (specular != other.specular)
				{
					return false;
				}

				if (renderQueue != other.renderQueue)
				{
					return false;
				}

				if (propFloats.Length != other.propFloats.Length)
				{
					return false;
				}

				if (propVectors.Length != other.propVectors.Length)
				{
					return false;
				}

				if (keywords.Length != other.keywords.Length)
				{
					return false;
				}

				for (int i = 0; i < propFloats.Length; i++)
				{
					if (propFloats[i].Equals(other.propFloats[i]) == false)
					{
						return false;
					}
				}

				for (int i = 0; i < propVectors.Length; i++)
				{
					if (propVectors[i].Equals(other.propVectors[i]) == false)
					{
						return false;
					}
				}

				for (int i = 0; i < keywords.Length; i++)
				{
					if (keywords[i] != other.keywords[i])
					{
						return false;
					}
				}

				return true;
			}

			public void GetTextureList(HashSet<string> list)
			{
				void Add(string name)
				{
					if (!string.IsNullOrEmpty(name))
					{
						list.Add(BASE_PATH_REMOVE + name);
					}
				}

				Add(albedo);
				Add(normal);
				Add(emission);
				Add(metal);
				Add(specular);
			}
		}

#if UNITY_EDITOR

		public static int g_MaterialIds = 0;

		public static Description[] Create(Renderer renderer, string source)
		{
			return Create(renderer.sharedMaterials, source);
		}

		public static Description[] Create(Material[] materials, string source)
		{
			Description[] result = new Description[materials.Length];

			for (int i = 0; i < result.Length; i++)
			{
				Material mat = materials[i];

				Description info = Create(mat, source);
				result[i] = info;
			}

			return result;
		}

		public static Description Create(Material mat, string source)
		{
			if (mat == null)
			{
				return new Description(Description.INVALID_MATERIAL, SHADER_ERROR, string.Empty);
			}

			if (mat.shader == null)
			{
				return new Description(Description.INVALID_MATERIAL, SHADER_ERROR, "null");
			}

			string warriorShaderName = GetWarriorShaderName(mat);

			if (warriorShaderName == SHADER_ERROR)
			{
				return new Description(Description.INVALID_MATERIAL, SHADER_ERROR, mat.shader.name);
			}

			Shader shader = mat.shader;
			string albedo = string.Empty;
			string normal = string.Empty;
			string emission = string.Empty;
			string metal = string.Empty;
			string specular = string.Empty;
			int renderQueue = mat.renderQueue;

			switch (shader.name)
			{
				case "Universal Render Pipeline/Simple Lit":
					albedo = GetTexturePath(mat.GetTexture(PROP_ALBEDO));
					normal = GetTexturePath(mat.GetTexture(PROP_NORMAL));
					emission = GetTexturePath(mat.GetTexture(PROP_EMISSION));
					specular = GetTexturePath(mat.GetTexture(PROP_SPECULAR));
					break;
				case "Universal Render Pipeline/Lit":
					albedo = GetTexturePath(mat.GetTexture(PROP_ALBEDO));
					normal = GetTexturePath(mat.GetTexture(PROP_NORMAL));
					emission = GetTexturePath(mat.GetTexture(PROP_EMISSION));
					metal = GetTexturePath(mat.GetTexture(PROP_METALLIC));
					specular = GetTexturePath(mat.GetTexture(PROP_SPECULAR));
					break;
				case "Universal Render Pipeline/Particles/Simple Lit":
					albedo = GetTexturePath(mat.GetTexture(PROP_ALBEDO));
					break;
				case "Warrior/Particle Cloud":
					albedo = GetTexturePath(mat.GetTexture(PROP_ALBEDO));
					break;
				case "Warrior/Water":
					normal = GetTexturePath(mat.GetTexture(PROP_WATER_NORMAL_A));
					albedo = GetTexturePath(mat.GetTexture(PROP_WATER_NORMAL_B));
					break;
				default:
					albedo = GetTexturePath(mat.GetTexture(PROP_ALBEDO));
					break;
			}

			GetMaterialsProperties(mat, out PropKeyValueVector[] vectors, out PropKeyValueFloat[] floats);

			g_MaterialIds++;
			return new Description($"{source}{g_MaterialIds}", warriorShaderName, shader.name,
				albedo, normal, emission, metal, specular, renderQueue, floats, vectors, GetMaterialKeywords(mat));
		}

		private static string[] GetMaterialKeywords(Material mat)
		{
			List<string> keywords = new List<string>(8);

			foreach (var keyword in mat.enabledKeywords)
			{
				keywords.Add(keyword.name);
			}

			return keywords.ToArray();
		}

		private static void GetMaterialsProperties(Material mat,
			out PropKeyValueVector[] vectors, out PropKeyValueFloat[] floats)
		{
			List<PropKeyValueVector> vectorResults = new List<PropKeyValueVector>(8);
			List<PropKeyValueFloat> floatResults = new List<PropKeyValueFloat>(8);

			string[] floatNames = mat.GetPropertyNames(MaterialPropertyType.Float);
			string[] vectorNames = mat.GetPropertyNames(MaterialPropertyType.Vector);

			foreach (string floatName in floatNames)
			{
				floatResults.Add(new PropKeyValueFloat(floatName, mat.GetFloat(floatName)));
			}

			foreach (string vectorName in vectorNames)
			{
				vectorResults.Add(new PropKeyValueVector(vectorName, mat.GetVector(vectorName)));
			}

			vectors = vectorResults.ToArray();
			floats = floatResults.ToArray();
		}

		private static string FixTexturePath(string texturePath, string pathToRemove)
		{
			if (string.IsNullOrEmpty(texturePath))
			{
				return string.Empty;
			}

			if (!texturePath.StartsWith(pathToRemove))
			{
				return texturePath;
			}

			return texturePath.Remove(0, pathToRemove.Length);
		}

		private static string GetTexturePath(Texture texture)
		{
			if (texture == null)
			{
				return string.Empty;
			}

			string result = AssetDatabase.GetAssetPath(texture);

			return FixTexturePath(result, BASE_PATH_REMOVE).ToLower();
		}

		private static string GetWarriorShaderName(Material mat)
		{
			if (mat.shader == null)
			{
				return SHADER_ERROR;
			}

			Shader shader = mat.shader;

			switch (shader.name)
			{
				case "Universal Render Pipeline/Simple Lit":
				case "Universal Render Pipeline/Lit":
					{
						if (ContainstKeyword(mat, "LIGHTMAP_ON"))
						{
							return SHADER_STATIC;
						}
						if (ContainstKeyword(mat, "_SURFACE_TYPE_TRANSPARENT"))
						{
							return SHADER_DYN_TRANSPARENT;
						}
						return SHADER_DYN_OPAQUE;
					}
				case "Universal Render Pipeline/Particles/Simple Lit":
					{
						return SHADER_PARTICLE;
					}
				case "Universal Render Pipeline/Particles/Unlit":
					{
						return SHADER_PARTICLE_UNLIT;
					}
				case "Warrior/Particle Cloud":
					{
						return SHADER_PARTICLE_CLOUD;
					}
				case "Warrior/Water":
					{
						return SHADER_WATER;
					}
				case "Legacy Shaders/Particles/Alpha Blended":
				case "Legacy Shaders/Particles/Alpha Blended Premultiply":
				case "Legacy Shaders/Particles/Additive":
				case "Particles/Standard Unlit":
					{
						return SHADER_ERROR;
					}
			}

			return SHADER_STATIC;
		}

		private static bool ContainstKeyword(Material mat, string keyword)
		{
			return Array.Exists(mat.shaderKeywords, x => x == keyword);
		}
#endif
	}
}
