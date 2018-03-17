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
	public List<Vector3> path;

	public void Start() {
		this.InitializePath();
		this.UpdateMesh();
	}

	public void InitializePath() {
		int count = this.nodesContainer.childCount;
		for (int i = 0; i < count; i++) {
			this.path.Add(this.nodesContainer.GetChild(i).transform.position);
		}
	}

	public void UpdateMesh() {
		this.ClearMesh();
		this.CreateMesh();
	}

	public void CreateMesh() {
		for (int i = 0; i < this.path.Count; i++) {
			Vector3 A = this.path[i];
			Vector3 B = this.path[(i + 1) % this.path.Count];
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
		int count = this.nodesContainer.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(this.nodesContainer.GetChild(i).gameObject);
		}
	}

	public float Length() {
		float l = 0f;
		for (int i = 0; i < this.path.Count; i++) {
			Vector3 A = this.path[i];
			Vector3 B = this.path[(i + 1) % this.path.Count];
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
		Vector3 B = this.path[(index + 1) % this.path.Count];
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
			B = this.path[(index + 1) % this.path.Count];
			tmpL += d;
			d = Vector3.Distance(A, B);
		}
		return index;
	}

	public Vector3? SegmentIntersection(Vector3 A, Vector3 B) {
		for (int i = 0; i < this.path.Count; i++) {
			Vector3? intersection = this.SegmentIntersectionIndex(A, B, i);
			if (intersection != null) {
				return intersection;
			}
		}
		return null;
	}

	public Vector3? SegmentIntersectionIndex(Vector3 A, Vector3 B, int index) {
		Vector3 fenceA = this.path[index];
		Vector3 fenceB = this.path[(index + 1) % this.path.Count];

        float x1 = A.x;
        float y1 = A.z;
        float x2 = B.x;
        float y2 = B.z;
        float x3 = fenceA.x;
        float y3 = fenceA.z;
        float x4 = fenceB.x;
        float y4 = fenceB.z;

        float det = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

		if (det == 0) {
			return null;
		}
		
		float x = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
		x = x / det;
		float y = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);
		y = y / det;
		Vector3 I = new Vector3(x, 0, y);

		if (Vector3.Dot(B - I, B - A) < 0f) {
			return null;
		}
		if (Vector3.Dot(A - I, A - B) < 0f) {
			return null;
		}
		if (Vector3.Dot(fenceB - I, fenceB - fenceA) < 0f) {
			return null;
		}
		if (Vector3.Dot(fenceA - I, fenceA - fenceB) < 0f) {
			return null;
		}

        return I;
	}

	public bool IsInside(Vector3 p) {
		int count = 0;
		for (int i = 0; i < this.path.Count; i++) {
			Vector3? intersection = this.SegmentIntersectionIndex(p, new Vector3(42, 0, 42), i);
			if (intersection != null) {
				count++;
			}
		}
		return (count % 2 == 1);
	}
}
