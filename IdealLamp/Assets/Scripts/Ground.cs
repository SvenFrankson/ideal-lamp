using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public int[][] grid;
	private int width;
	private int height;

	void Start () {
		this.width = Mathf.FloorToInt(this.transform.localScale.x);
		this.height = Mathf.FloorToInt(this.transform.localScale.z);

		grid = new int[width][];
		for (int i = 0; i < width; i++) {
			grid[i] = new int[height];
			for (int j = 0; j < width; j++) {
				grid[i][j] = Random.Range(0, 2);
			}
		}

		this.transform.localPosition = new Vector3(- this.width / 2, 0, - this.height / 2);
		this.transform.localScale = Vector3.one;

		this.GetComponent<MeshFilter>().mesh = this.CreateMesh();
	}

	public Vector2[] uvsForABCD(int a, int b, int c, int d) {
        string s = "" + a + b + c + d;
		Debug.Log(s);
        if (s == "0000") { // 1
            return new Vector2[] { new Vector2(1f / 3f, 2f / 3f), new Vector2(1f / 3f, 1), new Vector2(2f / 3f, 1), new Vector2(2f / 3f, 2f / 3f) };
        }
        if (s == "0001") { // 2
            return new Vector2[] { new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1), new Vector2(1, 1), new Vector2(1, 2f / 3f) };
        }
        if (s == "0010") {
            return new Vector2[] { new Vector2(2f / 3f, 1), new Vector2(1, 1), new Vector2(1, 2f / 3f), new Vector2(2f / 3f, 2f / 3f) };
        }
        if (s == "0011") {
            return new Vector2[] { new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1f / 3f), new Vector2(1f / 3f, 1f / 3f), new Vector2(1f / 3f, 2f / 3f) };
        }
        if (s == "0100") {
            return new Vector2[] { new Vector2(1, 1), new Vector2(1, 2f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1) };
        }
        if (s == "0101") { // 5
            return new Vector2[] { new Vector2(2f / 3f, 1f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(1, 2f / 3f), new Vector2(1, 1f / 3f) };
        }
        if (s == "0110") {
            return new Vector2[] { new Vector2(2f / 3f, 1f / 3f), new Vector2(1f / 3f, 1f / 3f), new Vector2(1f / 3f, 2f / 3f), new Vector2(2f / 3f, 2f / 3f) };
        }
        if (s == "0111") {
            return new Vector2[] { new Vector2(1f / 3f, 1f / 3f), new Vector2(0, 1f / 3f), new Vector2(0, 2f / 3f), new Vector2(1f / 3f, 2f / 3f) };
        }
        if (s == "1000") {
            return new Vector2[] { new Vector2(1, 2f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1), new Vector2(1, 1) };
        }
        if (s == "1001") {
            return new Vector2[] { new Vector2(1f / 3f, 2f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1f / 3f), new Vector2(1f / 3f, 1f / 3f) };
        }
        if (s == "1010") {
            return new Vector2[] { new Vector2(1, 1f / 3f), new Vector2(2f / 3f, 1f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(1, 2f / 3f) };
        }
        if (s == "1011") {
            return new Vector2[] { new Vector2(1f / 3f, 2f / 3f), new Vector2(1f / 3f, 1f / 3f), new Vector2(0, 1f / 3f), new Vector2(0, 2f / 3f) };
        }
        if (s == "1100") { // 4
            return new Vector2[] { new Vector2(1f / 3f, 1f / 3f), new Vector2(1f / 3f, 2f / 3f), new Vector2(2f / 3f, 2f / 3f), new Vector2(2f / 3f, 1f / 3f) };
        }
        if (s == "1101") {
            return new Vector2[] { new Vector2(0, 2f / 3f), new Vector2(1f / 3f, 2f / 3f), new Vector2(1f / 3f, 1f / 3f), new Vector2(0, 1f / 3f) };
        }
        if (s == "1110") { // 3
            return new Vector2[] { new Vector2(0, 1f / 3f), new Vector2(0, 2f / 3f), new Vector2(1f / 3f, 2f / 3f), new Vector2(1f / 3f, 1f / 3f) };
        }
        if (s == "1111") { // 0
            return new Vector2[] { new Vector2(0, 2f / 3f), new Vector2(0, 1), new Vector2(1f / 3f, 1), new Vector2(1f / 3f, 2f / 3f) };
        }
        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    }
	
	public Mesh CreateMesh() {
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		for (int i = 0; i < this.width - 1; i++) {
			for (int j = 0; j < this.height - 1; j++) {
				int l = vertices.Count;

				vertices.Add(new Vector3(i, 0, j));
				vertices.Add(new Vector3(i, 0, j + 1));
				vertices.Add(new Vector3(i + 1, 0, j + 1));
				vertices.Add(new Vector3(i + 1, 0, j));

				triangles.Add(l);
				triangles.Add(l + 1);
				triangles.Add(l + 2);
				triangles.Add(l);
				triangles.Add(l + 2);
				triangles.Add(l + 3);

				Vector2[] rawUvs = this.uvsForABCD(this.grid[i][j + 1], this.grid[i + 1][j + 1], this.grid[i + 1][j], this.grid[i][j]);
				//rawUvs[0] += new Vector2(0.01f, 0.01f);
				//rawUvs[1] += new Vector2(0.01f, - 0.01f);
				//rawUvs[2] += new Vector2(- 0.01f, - 0.01f);
				//rawUvs[3] += new Vector2(- 0.01f, 0.01f);
				uvs.AddRange(rawUvs);
			}
		}

		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		return mesh;
	}
}
