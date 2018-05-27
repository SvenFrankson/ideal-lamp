using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FenceBuilder {
    
    public static Mesh FenceMeshFromPath(List<Vector3> path) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        path.ForEach(
            (p) => {
                AddPole(vertices, triangles, uvs, p);
            }
        );

        Mesh m = new Mesh();

        m.vertices = vertices.ToArray();
        m.uv = uvs.ToArray();
        m.triangles = triangles.ToArray();

        return m;
    }

    private static void AddPole(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, Vector3 p) {
        Vector3 a = p - new Vector3(- 0.1f, 0, - 0.3f);
        Vector3 b = p - new Vector3(- 0.1f, 0, + 0.3f);
        Vector3 c = p - new Vector3(+ 0.1f, 0, + 0.3f);
        Vector3 d = p - new Vector3(+ 0.1f, 0, - 0.3f);

        int l = vertices.Count;

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);

        triangles.Add(l);
        triangles.Add(l + 1);
        triangles.Add(l + 2);
        triangles.Add(l);
        triangles.Add(l + 2);
        triangles.Add(l + 3);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
    }
}