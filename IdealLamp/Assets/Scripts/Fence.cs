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

	public bool killOutedDogs = true;
	public bool killOutedChickens = true;

	private Vector3 _min = Vector3.zero;
	public Vector3 min {
		get {
			return this._min;
		}
	}
	private Vector3 _max = Vector3.zero;
	public Vector3 max {
		get {
			return this._max;
		}
	}

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
		
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		float minZ = float.MaxValue;
		float maxZ = float.MinValue;

		for (int i = 0; i < this.path.Count; i++) {
			minX = Mathf.Min(minX, this.path[i].x);
			maxX = Mathf.Max(maxX, this.path[i].x);
			minZ = Mathf.Min(minZ, this.path[i].z);
			maxZ = Mathf.Max(maxZ, this.path[i].z);
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
		
		this._min = new Vector3(minX, 0, minZ);
		this._max = new Vector3(maxX, 0, maxZ);
	}
	
	public Vector3 DirAt(int index) {
		Vector3 A = this.path[index];
		Vector3 B = this.path[(index + 1) % this.path.Count];

		return (B - A).normalized;
	}
	
	public Vector3 NormalAt(int index) {
		Vector3 A = this.path[index];
		Vector3 B = this.path[(index + 1) % this.path.Count];

		Vector3 normal = Quaternion.AngleAxis(-90, Vector3.up) * (B - A).normalized;

		return normal;
	}

	public int Split(int indexStart, int indexEnd, List<Vector3> pathCut, ref int fenceRotation) {
		if (indexStart < indexEnd) {
			if (indexEnd - indexStart > this.path.Count / 2) {
				fenceRotation = -1;
				Debug.Log("Split case 10 (Forward)");
				this.path = this.path.GetRange(indexStart + 1, indexEnd - indexStart);
				for (int i = pathCut.Count - 1; i >= 0; i--) {
					this.path.Add(pathCut[i]);
				}
			} else {
				fenceRotation = 1;
				Debug.Log("Split case 11 (Forward inverted)");
				this.path.RemoveRange(indexStart + 1, indexEnd - indexStart);
				this.path.InsertRange(indexStart + 1, pathCut);
			}
		}
		else if (indexStart > indexEnd) {
			if (indexStart - indexEnd > this.path.Count / 2) {
				fenceRotation = 1;
				Debug.Log("Split case 20 (Backward)");
				this.path = this.path.GetRange(indexEnd + 1, indexStart - indexEnd);
				for (int i = 0; i < pathCut.Count; i++) {
					this.path.Add(pathCut[i]);
				}
			} else {
				fenceRotation = -1;
				Debug.Log("Split case 21 (Backward inverted)");
				List<Vector3> invertedPathCut = new List<Vector3>();
				for (int i = pathCut.Count - 1; i >= 0; i--) {
					invertedPathCut.Add(pathCut[i]);
				}
				this.path.RemoveRange(indexEnd + 1, indexStart - indexEnd);
				this.path.InsertRange(indexEnd + 1, invertedPathCut);
			}
		}
		else if (indexStart == indexEnd) {
			if (Vector3.Distance(pathCut[0], this.path[indexStart]) < Vector3.Distance(pathCut[pathCut.Count - 1], this.path[indexStart])) {
				Debug.Log("Split case 00 (Same segment)");
				this.path.InsertRange(indexStart + 1, pathCut);
			} else {
				fenceRotation = -1;
				Debug.Log("Split case 01 (Same segment Inverted)");
				List<Vector3> invertedPathCut = new List<Vector3>();
				for (int i = pathCut.Count - 1; i >= 0; i--) {
					invertedPathCut.Add(pathCut[i]);
				}
				this.path.InsertRange(indexStart + 1, invertedPathCut);
			}
		}
		else {
			return indexStart;
		}
		this.UpdateLength();
		if (this.killOutedChickens) {
			this.CheckChickens();
		}
		if (this.killOutedDogs) {
			this.CheckDogs();
		}
		this.UpdateMesh();
		return this.path.IndexOf(pathCut[pathCut.Count - 1]);
	}

	public void CheckChickens() {
		Chicken.instances.ForEach(
			(c) => {
				if (!this.IsInside(c.transform.position)) {
					Destroy(c.gameObject);
					Debug.Log("Success ! One Chicken out !");
				}
			}
		);
	}

	public void CheckDogs() {
		Dog.instances.ForEach(
			(d) => {
				if (!this.IsInside(d.transform.position)) {
					Destroy(d.gameObject);
					Debug.Log("Success ! One Dog out !");
				}
			}
		);
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

        return SegmentMath.SegmentSegmentIntersection(A, B, fenceA, fenceB);
	}

	public bool IsInside(Vector3 p) {
		return SegmentMath.IsInsidePath(p, this.path);
	}

	public Vector3 RandomInsidePosition(Vector3? rMin = null, Vector3? rMax = null) {
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

		if (rMin != null) {
			minX = Mathf.Max(rMin.GetValueOrDefault().x);
			minZ = Mathf.Max(rMin.GetValueOrDefault().z);
		}
		if (rMax != null) {
			maxX = Mathf.Min(rMax.GetValueOrDefault().x);
			maxZ = Mathf.Min(rMax.GetValueOrDefault().z);
		}

		int max = 0;
		bool inside = false;
		Vector3 random = Vector3.zero;
		while (max < 100 && !inside) {
			max++;
			random = new Vector3(
				Random.Range(minX, maxX),
				0f,  
				Random.Range(minZ, maxZ)
			);
			if (this.IsInside(random)) {
				Debug.Log("Ok, its inside");
				inside = true;
			}
		}
		if (!inside) {
			Debug.LogWarning("Careful, inside Vector is returned actually outside.");
		}
		return random;
	}
}
