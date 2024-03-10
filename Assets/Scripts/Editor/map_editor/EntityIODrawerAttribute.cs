#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class EntityIODrawerAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EntityIODrawerAttribute))]
public class EntityIODrawer : PropertyDrawer
{
	// WIP..
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		float width = position.width / 5.0f;
		float delayWidth = width / 2.0f;
		float padding = 4.0f;

		Rect outputNameRect = new Rect(position.x + padding, position.y, width - padding, position.height);
		Rect targetRect = new Rect(position.x + width + padding, position.y, width - padding, position.height);
		Rect inputNameRect = new Rect(position.x + width * 2.0f + padding, position.y, width - padding, position.height);
		Rect paramRect = new Rect(position.x + width * 3.0f + padding, position.y, width - padding, position.height);
		Rect delayRect = new Rect(position.x + width * 4.0f + padding, position.y, delayWidth - padding, position.height);

		GetInputOutput(property.stringValue, out string outputName, out string input);
		string[] options = input.Split(",");

		EditorGUI.BeginChangeCheck();

		int iOutput = EditorGUI.Popup(outputNameRect, GetOutputIndex(outputName), EntityIOList.OUTPUTS);
		string target = EditorGUI.TextField(targetRect, GetOptionParam(options, 0));
		int iInput = EditorGUI.Popup(inputNameRect, GetInputIndex(GetOptionParam(options, 1)), EntityIOList.INPUTS);
		string paramString = EditorGUI.TextField(paramRect, GetOptionParam(options, 2));
		float delay = EditorGUI.FloatField(delayRect, GetDelayValue(GetOptionParam(options, 3)));

		if (EditorGUI.EndChangeCheck())
		{
			property.stringValue = EntityIOList.OUTPUTS[iOutput] +
				$" {target},{EntityIOList.INPUTS[iInput]},{paramString},{Utils.FloatToString(delay)}";
		}
	}

	private static float GetDelayValue(string delayString)
	{
		return Utils.StringToFloat(delayString);
	}

	private static int GetInputIndex(string inputName)
	{
		int idx = System.Array.FindIndex(EntityIOList.INPUTS, x => x == inputName);
		return idx != -1 ? idx : 0;
	}

	private static int GetOutputIndex(string outputName)
	{
		int idx = System.Array.FindIndex(EntityIOList.OUTPUTS, x => x == outputName);
		return idx != -1 ? idx : 0;
	}

	private static string GetOptionParam(string[] options, int idx)
	{
		if (idx < options.Length)
		{
			return options[idx];
		}

		return "";
	}

	private static void GetInputOutput(string param, out string outputName, out string input)
	{
		int idx = param.IndexOf(' ');

		if (idx != -1)
		{
			outputName = param.Substring(0, idx);
			input = param.Substring(idx + 1);
		}
		else
		{
			outputName = "";
			input = "";
		}
	}
}
#endif