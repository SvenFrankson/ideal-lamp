using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour {

	private Fence _fence;
	public Fence fence {
		get {
			if (!this._fence) {
				this._fence = FindObjectOfType<Fence>();
			}
			return this._fence;
		}
	}

	private Transform _nodesContainer;
	private Transform nodesContainer {
		get {
			if (this._nodesContainer == null) {
				this._nodesContainer = GameObject.Find("FoxNodesContainer").transform;
			}
			return this._nodesContainer;
		}
	}

	public bool isAlive = true;
	public bool isOnFence = true;
	public int fenceRotation = 1;
	public float fencePos = 0f;

	private Vector3 currentDir = Vector3.zero;
	private List<Vector3> foxCut = new List<Vector3>();
	private int cutIndexStart = 0;
	private int cutIndexEnd = 0;

	public float speed = 0f;
	public bool canMoveFree = true;

	public Vector3 FenceDirection() {
		int nodeIndex = this.fence.FencePosToNodeIndex(this.fencePos);
		return this.fence.DirAt(nodeIndex);
	}

	public List<Vector3> GetPath() {
		if (this.foxCut.Count == 0) {
			return new List<Vector3>();
		}
		else {
			List<Vector3> path = new List<Vector3>(this.foxCut);
			path.Add(this.transform.position);
			return path;
		}
	}

	public void Update() {
		if (this.isAlive) {
			if (this.isOnFence) {
				this._onFenceUpdate();
			}
			else {
				this._inFieldUpdate();
			}
		}
		this.UpdateMesh();
	}

	private void _onFenceUpdate() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			Vector3 fenceDirection = this.FenceDirection();
			if (fenceDirection.z == 1f) {
				fenceRotation = 1;
				return;
			}
			if (fenceDirection.z == -1f) {
				fenceRotation = -1;
				return;
			}
			if (fenceDirection.x == -1f) {
				return;
			}
			this.foxCut = new List<Vector3>();
			this.cutIndexStart = this.fence.FencePosToNodeIndex(this.fencePos);
			this.isOnFence = false;
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.forward;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Vector3 fenceDirection = this.FenceDirection();
			if (fenceDirection.z == -1f) {
				fenceRotation = 1;
				return;
			}
			if (fenceDirection.z == 1f) {
				fenceRotation = -1;
				return;
			}
			if (fenceDirection.x == 1f) {
				return;
			}
			this.foxCut = new List<Vector3>();
			this.cutIndexStart = this.fence.FencePosToNodeIndex(this.fencePos);
			this.isOnFence = false;
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.back;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			Vector3 fenceDirection = this.FenceDirection();
			if (fenceDirection.x == 1f) {
				fenceRotation = 1;
				return;
			}
			if (fenceDirection.x == -1f) {
				fenceRotation = -1;
				return;
			}
			if (fenceDirection.z == 1f) {
				return;
			}
			this.foxCut = new List<Vector3>();
			this.cutIndexStart = this.fence.FencePosToNodeIndex(this.fencePos);
			this.isOnFence = false;
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.right;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Vector3 fenceDirection = this.FenceDirection();
			if (fenceDirection.x == -1f) {
				fenceRotation = 1;
				return;
			}
			if (fenceDirection.x == 1f) {
				fenceRotation = -1;
				return;
			}
			if (fenceDirection.z == -1f) {
				return;
			}
			this.foxCut = new List<Vector3>();
			this.cutIndexStart = this.fence.FencePosToNodeIndex(this.fencePos);
			this.isOnFence = false;
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.left;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		this.transform.position = this.fence.FencePosToWorldPos(this.fencePos);
		this.fencePos += this.fenceRotation * this.speed * Time.deltaTime;
		while (this.fencePos >= this.fence.totalLength) {
			this.fencePos -= this.fence.totalLength;
		}
		while (this.fencePos < 0f) {
			this.fencePos += this.fence.totalLength;
		}
	}

	private void _inFieldUpdate() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (this.currentDir.x == 0f) {
				return;
			}
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.forward;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if (this.currentDir.x == 0f) {
				return;
			}
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.back;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (this.currentDir.z == 0f) {
				return;
			}
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.right;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (this.currentDir.z == 0f) {
				return;
			}
			this.foxCut.Add(this.transform.position);
			this.currentDir = Vector3.left;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		Vector3 previousPos = this.transform.position;
		this.transform.position += this.currentDir * this.speed * Time.deltaTime;
		int intersectionIndex;
		Vector3? fenceIntersection = this.fence.SegmentIntersection(previousPos, this.transform.position, out intersectionIndex);
		if (fenceIntersection != null) {
			Debug.Log("Fox joins fence at index '" + intersectionIndex + "'");
			this.isOnFence = true;
			this.foxCut.Add(fenceIntersection.GetValueOrDefault());
			this.cutIndexEnd = intersectionIndex;
			intersectionIndex = this.fence.Split(this.cutIndexStart, this.cutIndexEnd, this.foxCut, ref this.fenceRotation);
			this.fencePos = this.fence.WorldPosAndIndexToFencePos(this.transform.position, intersectionIndex);
			this.foxCut = new List<Vector3>();
			Debug.Log("FoxCut is made of " + this.foxCut.Count + " steps.");
		}
	}

	public void UpdateMesh() {
		this.ClearMesh();
		this.CreateMesh();
	}

	public void CreateMesh() {
		if (this.foxCut.Count == 0) {
			return;
		}
		List<Vector3> path = this.GetPath();
		for (int i = 0; i < path.Count - 1; i++) {
			Vector3 A = path[i];
			Vector3 B = path[i + 1];
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
			if (this.isAlive) {
				fencePart.GetComponent<MeshRenderer>().material = TestMaterials.instance.fence;
			}
			else {
				fencePart.GetComponent<MeshRenderer>().material = TestMaterials.instance.destroyedFence;
			}
		}
	}

	public void ClearMesh() {
		int count = this.nodesContainer.childCount;
		for (int i = 0; i < count; i++) {
			Destroy(this.nodesContainer.GetChild(i).gameObject);
		}
	}
}
