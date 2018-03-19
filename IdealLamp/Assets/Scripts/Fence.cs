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
	public List<float> sumLength;
	public float totalLength;

	public void Start() {
		this.InitializePath();
		this.UpdateMesh();
	}

	public void InitializePath() {
		int count = this.nodesContainer.childCount;
		for (int i = 0; i < count; i++) {
			this.path.Add(this.nodesContainer.GetChild(i).transform.position);
		}
		this.UpdateLength();
	}

	public void UpdateLength() {
		this.sumLength = new List<float>();
		this.totalLength = 0f;
		for (int i = 0; i < this.path.Count; i++) {
			if (i > 0) {
				this.sumLength.Add(
					this.sumLength[i - 1] + Vector3.Distance(this.path[i], this.path[i - 1])
				);
			} else {
				this.sumLength.Add(0);
			}
			Vector3 A = this.path[i];
			Vector3 B = this.path[(i + 1) % this.path.Count];
			this.totalLength += Vector3.Distance(A, B);
		}
	}

	public void Split(int indexStart, int indexEnd, List<Vector3> pathCut) {
		if (indexStart < indexEnd) {
			if (indexEnd - indexStart > this.path.Count / 2) {
				Debug.Log("Split case 10 (Forward)");
				this.path = this.path.GetRange(indexStart + 1, indexEnd - indexStart);
				for (int i = pathCut.Count - 1; i >= 0; i--) {
					this.path.Add(pathCut[i]);
				}
			} else {
				Debug.Log("Split case 11 (Forward inverted)");
				this.path.RemoveRange(indexStart + 1, indexEnd - indexStart);
				this.path.InsertRange(indexStart + 1, pathCut);
			}
		}
		else if (indexStart > indexEnd) {
				Debug.Log("Split case 20 (Backward)");
			if (indexStart - indexEnd > this.path.Count / 2) {
				this.path = this.path.GetRange(indexEnd + 1, indexStart - indexEnd);
				for (int i = 0; i < pathCut.Count; i++) {
					this.path.Add(pathCut[i]);
				}
			} else {
				Debug.Log("Split case 21 (Backward inverted)");
				this.path.RemoveRange(indexEnd + 1, indexStart - indexEnd);
				this.path.InsertRange(indexEnd + 1, pathCut);
			}
		}
		this.UpdateLength();
		this.UpdateMesh();
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
		GameObject zero = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		zero.transform.position = this.path[0] + Vector3.up * 0.5f;
		zero.transform.localScale = Vector3.one * 0.2f;
		zero.transform.parent = this.nodesContainer;
	}

	public void ClearMesh() {
		int count = this.nodesContainer.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(this.nodesContainer.GetChild(i).gameObject);
		}
	}

	public Vector3 FencePosToWorldPos(float fencePos) {
		while (fencePos > this.totalLength) {
			fencePos -= this.totalLength;
		}
		while (fencePos < 0f) {
			fencePos += this.totalLength;
		}
		int index = this.FencePosToNodeIndex(fencePos);
		float deltaPos = fencePos - this.sumLength[index];
		Vector3 A = this.path[index];
		Vector3 B = this.path[(index + 1) % this.path.Count];
		return this.path[index] + (B - A).normalized * deltaPos;
	}

	public float WorldPosAndIndexToFencePos(Vector3 worldPos, int index) {
		return this.sumLength[index] + Vector3.Distance(this.path[index], worldPos);
	}

	public int FencePosToNodeIndex(float fencePos) {
		int index = this.path.Count - 1;
		while (fencePos >= this.totalLength) {
			fencePos -= this.totalLength;
		}
		while (fencePos < 0f) {
			fencePos += this.totalLength;
		}
		while (index > 0 && fencePos <= this.sumLength[index]) {
			index --;
		}
		return index;
	}

	public Vector3? SegmentIntersection(Vector3 A, Vector3 B, out int segmentIndex) {
		for (int i = 0; i < this.path.Count; i++) {
			Vector3? intersection = this.SegmentIntersectionIndex(A, B, i);
			if (intersection != null) {
				segmentIndex = i;
				return intersection;
			}
		}
		segmentIndex = -1;
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

	public Vector3 RandomInsidePosition() {
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		float minZ = float.MaxValue;
		float maxZ = float.MinValue;

		for (int i = 0; i < this.path.Count; i++) {
			minX = Mathf.Min(minX, this.path[i].x);
			maxX = Mathf.Max(maxX, this.path[i].x);
			minZ = Mathf.Min(minZ, this.path[i].z);
			maxZ = Mathf.Max(maxZ, this.path[i].z);
		}

		int max = 0;
		bool inside = false;
		Vector3 random = Vector3.zero;
		while (max < 10 && !inside) {
			max++;
			random = new Vector3(
				Random.Range(minX, maxX),
				0f,  
				Random.Range(minZ, maxZ)
			);
			if (this.IsInside(random)) {
				inside = true;
			}
		}
		if (!inside) {
			Debug.LogWarning("Careful, inside Vector is returned actually outside.");
		}
		return random;
	}
}
