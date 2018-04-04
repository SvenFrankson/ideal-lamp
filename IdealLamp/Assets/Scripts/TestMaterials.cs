using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaterials : MonoBehaviour {

	private static TestMaterials _instance;
	public static TestMaterials instance {
		get {
			if (!TestMaterials._instance) {
				TestMaterials._instance = FindObjectOfType<TestMaterials>();
			}
			return TestMaterials._instance;
		}
	}

	public Material fence;
	public Material destroyedFence;
}
