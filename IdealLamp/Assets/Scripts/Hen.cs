using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hen : MonoBehaviour {

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
	public void Update() {
        if (Vector3.Distance(this.transform.position, destination) < this.speed * Time.deltaTime) {
            destination = new Vector3(
                Random.Range(-3f, 3f),
                0f,  
                Random.Range(-3f, 3f)
            );
        }
        Vector3 dir = (destination - this.transform.position).normalized;
        this.transform.position += dir * this.speed * Time.deltaTime;
	}
}
