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
    private Vector3 destination;

	public void Start() {
		Chicken.instances.Add(this);
	}

	public void Update() {
        if (Vector3.Distance(this.transform.position, destination) < this.speed * Time.deltaTime) {
            this.destination = this.fence.RandomInsidePosition();
        }
        Vector3 dir = (destination - this.transform.position).normalized;
        this.transform.position += dir * this.speed * Time.deltaTime;
	}

	public void OnDestroy() {
		Chicken.instances.Remove(this);
	}
}