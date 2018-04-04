using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour {

	public void OnGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.TextArea("Time " + Time.timeSinceLevelLoad.ToString("0.0"));
		GUILayout.TextArea("Chickens " + Chicken.instances.Count);
		GUILayout.TextArea("Dogs " + Dog.instances.Count);
		GUILayout.EndHorizontal();
	}
}
