#region Using Directives

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class DebugHelper : MonoBehaviour
{
	private static DebugHelper instance;
	private static List<DebugData> debugDataSet = new List<DebugData>();
	private static List<DebugData> dynamicDebugDataSet = new List<DebugData>();
	private static KeyCode currentDrawKeyCode = KeyCode.F15;

	private Text text;
	
	public static DebugData AddLine(GameObject source, string debugString, Color color, KeyCode drawKeyCode = KeyCode.F15, int size = 14, bool isAttachedToObject = false)
	{
		if (instance == null || instance.Equals(null))
		{
			instance = new GameObject("DebugHelper").AddComponent<DebugHelper>();
			instance.tag = "DebugHelper";
		}

		DebugData data = new DebugData(debugString, source, color, size, drawKeyCode);
		
		if (!isAttachedToObject)
		{
			debugDataSet.Add(data);
		}
		else
		{
			dynamicDebugDataSet.Add(data);
		}
		
		return data;
	}

	public static void RemoveLine(ref DebugData data )
	{
		debugDataSet.Remove(data);
		dynamicDebugDataSet.Remove(data);

		if (debugDataSet.Count == 0 && dynamicDebugDataSet.Count == 0)
		{
			Destroy(instance);
		}
	}

	protected void Start()
	{
		gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		gameObject.AddComponent<CanvasScaler>();
		GameObject childObject = new GameObject("DebugText");
		childObject.transform.SetParent(transform);
		text = childObject.AddComponent<Text>();
		text.rectTransform.anchorMax = new Vector2(1,1);
		text.rectTransform.anchorMin = new Vector2(0,0);
		text.rectTransform.pivot = new Vector2(0.5f,0.5f);
		text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
		text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
		text.rectTransform.offsetMax = new Vector2(1, 1);
		text.rectTransform.offsetMin = new Vector2(0, 0);

		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		text.font = ArialFont;
		text.fontSize = 7;
		text.enabled = true;
		text.color = Color.white;
	}

	protected void Update()
	{
		for (int i = 1; i < 13; i++) 
		{
			KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), "F" + i);

			if (Input.GetKeyDown(code))
			{
				if (currentDrawKeyCode == code) currentDrawKeyCode = KeyCode.F15;
				else currentDrawKeyCode = code;
			}
		}


		string debugTextString = "";

		foreach (var debugData in debugDataSet)
		{
			string color = ColorUtility.ToHtmlStringRGBA(debugData.color);
			string debugString = debugData.debugString;

			debugTextString += string.Format("<size={2}><color=#{0}>{1}</color></size>", color, debugString, debugData.size) + "\n\n";
		}

		text.text = debugTextString;
	}

	protected void OnGUI()
	{
		if (Camera.main == null) { return; }
		dynamicDebugDataSet.RemoveAll(data => data.sourceObject == null || data.sourceObject.Equals(null));
		
		foreach (var debugData in dynamicDebugDataSet)
		{
			if (currentDrawKeyCode != debugData.drawKeyCode)
			{
				continue;
			}
			Vector2 pos = Camera.main.WorldToScreenPoint(debugData.sourceObject.transform.position);
			pos.y = Camera.main.pixelHeight - pos.y;
			Vector3 size = new Vector3(100, 100);
			GUI.Label(new Rect(pos + new Vector2(1, 1), size), string.Format("<size={2}><color=#{0}>{1}</color></size>", ColorUtility.ToHtmlStringRGBA(Color.black), debugData.debugString, debugData.size));
			GUI.Label(new Rect(pos, size), string.Format("<size={2}><color=#{0}>{1}</color></size>", ColorUtility.ToHtmlStringRGBA(debugData.color), debugData.debugString, debugData.size));
		}
	}

	public class DebugData
	{
		public string debugString;
		public GameObject sourceObject;
		public Color color;
		public int size;
		public KeyCode drawKeyCode;

		internal DebugData(string debugString, GameObject sourceObject, Color color, int size, KeyCode drawKeyCode)
		{
			this.debugString = debugString;
			this.sourceObject = sourceObject;
			this.color = color;
			this.size = size;
			this.drawKeyCode = drawKeyCode;
		}
	}
}
