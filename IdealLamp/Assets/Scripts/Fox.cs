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

	public bool isOnFence = true;
	public int fenceRotation = 1;
	public float fencePos = 0f;

	public Vector3 currentDir = Vector3.zero;

	public float speed = 0f;

	public Vector3 FenceDirection() {
		int nodeIndex = this.fence.FencePosToNodeIndex(this.fencePos);
		return (this.fence.path[(nodeIndex + 1) % this.fence.path.Count] - this.fence.path[nodeIndex]).normalized;
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (this.isOnFence) {
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
			}
			this.isOnFence = false;
			this.currentDir = Vector3.forward;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if (this.isOnFence) {
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
			}
			this.isOnFence = false;
			this.currentDir = Vector3.back;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (this.isOnFence) {
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
			}
			this.isOnFence = false;
			this.currentDir = Vector3.right;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (this.isOnFence) {
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
			}
			this.isOnFence = false;
			this.currentDir = Vector3.left;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}

		if (this.isOnFence) {
			this.transform.position = this.fence.FencePosToWorldPos(this.fencePos);
			this.fencePos += this.fenceRotation * this.speed * Time.deltaTime;
			while (this.fencePos >= this.fence.totalLength) {
				this.fencePos -= this.fence.totalLength;
			}
			while (this.fencePos < 0f) {
				this.fencePos += this.fence.totalLength;
			}
		} else {
			Vector3 previousPos = this.transform.position;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			int intersectionIndex;
			Vector3? fenceIntersection = this.fence.SegmentIntersection(previousPos, this.transform.position, out intersectionIndex);
			if (fenceIntersection != null) {
				Debug.Log("Fox joins fence at index '" + intersectionIndex + "'");
				this.isOnFence = true;
				this.fencePos = this.fence.WorldPosAndIndexToFencePos(fenceIntersection.GetValueOrDefault(), intersectionIndex);
			}
		}
	}
}
