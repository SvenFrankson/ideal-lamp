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
	public float fencePos = 0f;

	public Vector3 currentDir = Vector3.zero;

	public float speed = 0f;

	public void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			this.isOnFence = false;
			this.currentDir = Vector3.forward;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			this.isOnFence = false;
			this.currentDir = Vector3.back;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			this.isOnFence = false;
			this.currentDir = Vector3.right;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			this.isOnFence = false;
			this.currentDir = Vector3.left;
			return;
		}

		if (this.isOnFence) {
			this.transform.position = this.fence.FencePosToWorldPos(this.fencePos);
			this.fencePos += this.speed * Time.deltaTime;
		} else {
			Vector3 previousPos = this.transform.position;
			this.transform.position += this.currentDir * this.speed * Time.deltaTime;
			Vector3? intersectsFence = this.fence.SegmentIntersection(previousPos, this.transform.position);
			if (intersectsFence != null) {
				this.isOnFence = true;
				this.fencePos = 0f;
			}
		}
	}
}
