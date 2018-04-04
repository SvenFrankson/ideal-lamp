using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour {

	private Fox _fox;
	public Fox fox {
		get {
			if (!this._fox) {
				this._fox = FindObjectOfType<Fox>();
			}
			return this._fox;
		}
	}

	public void OnGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.TextArea("Time " + Time.timeSinceLevelLoad.ToString("0.0"));
		GUILayout.TextArea("Chickens " + Chicken.instances.Count);
		GUILayout.TextArea("Dogs " + Dog.instances.Count);
		if (!this.fox.isAlive) {
			GUILayout.TextArea("GAME OVER");
		}
		GUILayout.EndHorizontal();
	}
}
