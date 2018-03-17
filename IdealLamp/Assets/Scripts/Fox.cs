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
	public float speed = 0f;

	public void Update() {
		if (this.isOnFence) {
			this.transform.position = this.fence.FencePosToWorldPos(this.fencePos);
			this.fencePos += this.speed * Time.deltaTime;
		}
	}
}
