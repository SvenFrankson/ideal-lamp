using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {

	private Transform _nodesContainer;
	private Transform nodesContainer {
		get {
			if (this._nodesContainer == null) {
				this._nodesContainer = this.transform.Find("NodesContainer");
			}
			return this._nodesContainer;
		}
	}
	public Vector3[] path;

	public void Start() {
		this.CreateMesh();
	}

	public void UpdateMesh() {
		this.ClearMesh();
		this.CreateMesh();
	}

	public void CreateMesh() {
		for (int i = 0; i < this.path.Length; i++) {
			Vector3 A = this.path[i];
			Vector3 B = this.path[(i + 1) % this.path.Length];
			GameObject fencePart = GameObject.CreatePrimitive(PrimitiveType.Cube);
			fencePart.transform.position = (B + A) * 0.5f;
			fencePart.transform.rotation = Quaternion.FromToRotation(
				Vector3.forward,
				(B - A).normalized
			);
			fencePart.transform.localScale = new Vector3(
				0.1f,
				0.8f,
				Vector3.Distance(B, A)
			);
			fencePart.transform.parent = this.nodesContainer;
		}
	}

	public void ClearMesh() {
		Transform child = this.transform.GetChild(0);
		while (child != null) {
			Destroy(child);
			child = this.transform.GetChild(0);
		}
	}

	public float Length() {
		float l = 0f;
		for (int i = 0; i < this.path.Length; i++) {
			Vector3 A = this.path[i];
			Vector3 B = this.path[(i + 1) % this.path.Length];
			l += Vector3.Distance(A, B);
		}
		return l;
	}

	public Vector3 FencePosToWorldPos(float fencePos) {
		float l = this.Length();
		while (fencePos > l) {
			fencePos -= l;
		}
		while (fencePos < 0f) {
			fencePos += l;
		}
		float tmpL = 0;
		int index = this.FencePosToNodeIndex(fencePos, out tmpL);
		float deltaPos = fencePos - tmpL;
		Vector3 A = this.path[index];
		Vector3 B = this.path[(index + 1) % this.path.Length];
		return this.path[index] + (B - A).normalized * deltaPos;
	}

	public int FencePosToNodeIndex(float fencePos, out float tmpL) {
		Vector3 A = this.path[0];
		Vector3 B = this.path[1];
		float d = Vector3.Distance(A, B);
		int index = 0;
		tmpL = 0f;
		while (tmpL + d < fencePos) {
			index ++;
			A = this.path[index];
			B = this.path[(index + 1) % this.path.Length];
			tmpL += d;
			d = Vector3.Distance(A, B);
		}
		return index;
	}
}
