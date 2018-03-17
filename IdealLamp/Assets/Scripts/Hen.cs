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
            int max = 0;
            bool inside = false;
            while (max < 10 || !inside) {
                max++;
                destination = new Vector3(
                    Random.Range(-5f, 5f),
                    0f,  
                    Random.Range(-5f, 5f)
                );
                if (this.fence.IsInside(this.destination)) {
                    inside = true;
                }
            }
            if (!inside) {
                Debug.LogWarning("CHICKEN OUT ! CHICKEN OUT !!!");
            }
        }
        Vector3 dir = (destination - this.transform.position).normalized;
        this.transform.position += dir * this.speed * Time.deltaTime;
	}
}
