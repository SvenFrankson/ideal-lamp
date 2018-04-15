using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour {

	public static List<Chicken> instances = new List<Chicken>();

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
	//public float rotationSmoothness;
    public Vector3 destination;

	public void Start() {
		Chicken.instances.Add(this);
	}

	public void Update() {
        if (Vector3.Distance(this.transform.position, destination) < this.speed * Time.deltaTime) {
            this.destination = this.fence.RandomInsidePosition(this.transform.position - new Vector3(2, 0, 2), this.transform.position + new Vector3(2, 0, 2));
        }
        Vector3 dir = this.destination - this.transform.position;
		dir.Normalize();
		this.transform.position += dir * this.speed * Time.deltaTime;
		//Quaternion rot = Quaternion.FromToRotation(Vector3.forward, this.destination - this.transform.position);
		//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, this.rotationSmoothness);
	}

	public void OnDestroy() {
		Chicken.instances.Remove(this);
	}
}
