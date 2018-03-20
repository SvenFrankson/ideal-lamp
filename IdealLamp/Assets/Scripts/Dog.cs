using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

	public static List<Dog> instances = new List<Dog>();

	private Fence _fence;
	public Fence fence {
		get {
			if (!this._fence) {
				this._fence = FindObjectOfType<Fence>();
			}
			return this._fence;
		}
	}

    public float speed;

	public void Start() {
		Dog.instances.Add(this);
	}

	public void Update() {
        Vector3 previousPos = this.transform.position;
        this.transform.position += this.transform.forward * this.speed * Time.deltaTime;
        int intersectionIndex;
        Vector3? fenceIntersection = this.fence.SegmentIntersection(previousPos, this.transform.position, out intersectionIndex);
        if (fenceIntersection != null) {
            Vector3 fenceNormal = this.fence.NormalAt(intersectionIndex);
            Vector3 newForward = Quaternion.AngleAxis(Vector3.Angle(this.transform.forward, fenceNormal) * 2f, Vector3.up) * - this.transform.forward;
            this.transform.position = fenceIntersection.GetValueOrDefault() + fenceNormal * 0.05f;
            this.transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);
        }
	}

	public void OnDestroy() {
		Dog.instances.Remove(this);
	}
}
