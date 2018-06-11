using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	private string[][] mapping6x6;
	public int[] grid;
	public int width;
	public int height;

	void Start () {
		
	}

	private void Initialize6x6() {
		this.mapping6x6 = new string[6][];
		this.mapping6x6[0] = new string[] {
			"1111",
			"1100",
			"0000",
			"0000",
			"0011",
			"1111"
		};
		this.mapping6x6[1] = new string[] {
			"1001",
			"1000",
			"0000",
			"0000",
			"0001",
			"1001"
		};
		this.mapping6x6[2] = new string[] {
			"0000",
			"0000",
			"0111",
			"1110",
			"0000",
			"0000"
		};
		this.mapping6x6[3] = new string[] {
			"0000",
			"0000",
			"1011",
			"1101",
			"0000",
			"0000"
		};
		this.mapping6x6[4] = new string[] {
			"0110",
			"0100",
			"0000",
			"0000",
			"0010",
			"0110"
		};
		this.mapping6x6[5] = new string[] {
			"1111",
			"1100",
			"0000",
			"0000",
			"0011",
			"1111"
		};
	}

	public Vector2[] IJTo6x6UV(int i, int j) {
		float fi = (float) i;
		float fj = (float) j;
		return new Vector2[] {
			new Vector2(fi / 6f, fj / 6f),
			new Vector2(fi / 6f, (fj + 1) / 6f),
			new Vector2((fi + 1) / 6f, (fj + 1) / 6f),
			new Vector2((fi + 1) / 6f, fj / 6f)
		};
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

	public Vector2 WorldPointToGrid(Vector3 world) {
		int i = Mathf.RoundToInt(world.x);
		int j = Mathf.RoundToInt(world.z);
		if (i >= 0 && i < this.width) {
			if (j >= 0 && j < this.height) {
				return new Vector2(i, j);
			}
		}
		return new Vector2(-1, -1);
	}

	public void UpdateGrid() {
		int[] newGrid = new int[this.width * this.height];
		for (int i = 0; i < this.width; i++) {
			for (int j = 0; j < this.height; j++) {
				newGrid[i + j * this.width] = 0;
			}
		}

		if (this.grid != null) {
			for (int i = 0; i < this.width; i++) {
				for (int j = 0; j < this.height; j++) {
					if (i + j * this.width < this.grid.Length) {
						newGrid[i + j * this.width] = grid[i + j * this.width];
					}
				}
			}
		}

		this.grid = newGrid;
	}
	
	public void UpdateMesh() {
		this.UpdateGrid();
		this.GetComponent<MeshFilter>().sharedMesh = this.CreateMesh();
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

				Vector2[] rawUvs = this.uvsForABCD(this.grid[i + (j + 1) * this.width], this.grid[i + 1 + (j + 1) * this.width], this.grid[i + 1 + j * this.width], this.grid[i + j * this.width]);
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
