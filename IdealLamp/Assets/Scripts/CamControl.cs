using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

	private Camera _cameraComponent;
	public Camera cameraComponent {
		get {
			if (!this._cameraComponent) {
				this._cameraComponent = FindObjectOfType<Camera>();
			}
			return this._cameraComponent;
		}
	}

	private Fence _fence;
	public Fence fence {
		get {
			if (!this._fence) {
				this._fence = FindObjectOfType<Fence>();
			}
			return this._fence;
		}
	}

	[Range(1, 100)]
	public int smoothness;
	private float _smoothFactor {
		get {
			return 1f / smoothness;
		}
	}
	public int edgeSize;
	
	void Update () {
		float newSizeX = (this.fence.max.x - this.fence.min.x) * 0.5f + this.edgeSize;
		float newSizeZ = (this.fence.max.z - this.fence.min.z) * 0.5f + this.edgeSize;
		float ratio = Screen.width / Screen.height;
		float newSize = Mathf.Max(newSizeX / ratio, newSizeZ);
		this.transform.position = Vector3.Lerp(
			this.transform.position,
			new Vector3(
				(this.fence.max.x + this.fence.min.x) * 0.5f,
				20,
				(this.fence.max.z + this.fence.min.z) * 0.5f
			),
			this._smoothFactor
		);
		cameraComponent.orthographicSize = cameraComponent.orthographicSize * (1f - this._smoothFactor) + newSize * this._smoothFactor;
	}
}
