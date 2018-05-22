using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Ground))]
public class GroundEditor : Editor {

    public void OnSceneGUI() {
		Event e = Event.current;

		if (e.type == EventType.MouseDown) {
			Debug.Log("Mouse Down");
			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			Plane plane = new Plane(Vector3.up, Vector3.zero);
			float d = 0;
			plane.Raycast(ray, out d);
			Vector3 point = ray.GetPoint(d);
			Ground ground = (Ground) this.target;
			Vector2 coordinates = ground.WorldPointToGrid(point);
			Debug.Log(coordinates);
			if (coordinates.x > -1) {
				if (ground.grid != null) {
					ground.grid[(int) coordinates.x + (int) coordinates.y * ground.width] += 1;
					ground.grid[(int) coordinates.x + (int) coordinates.y * ground.width] %= 2;
				}
				ground.UpdateMesh();
				e.Use();
			}
		}
    }
}