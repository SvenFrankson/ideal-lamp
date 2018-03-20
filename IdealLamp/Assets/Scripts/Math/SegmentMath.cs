using UnityEngine;
using System.Collections.Generic;

class SegmentMath {

    public static bool IsInsidePath(Vector3 p, List<Vector3> path) {
		int count = 0;
		for (int i = 0; i < path.Count; i++) {
		    Vector3 C = path[i];
		    Vector3 D = path[(i + 1) % path.Count];
			Vector3? intersection = SegmentSegmentIntersection(p, new Vector3(42, 0, 42), C, D);
			if (intersection != null) {
				count++;
			}
		}
		return (count % 2 == 1);
	}

    public static Vector3? SegmentPathIntersection(Vector3 A, Vector3 B, List<Vector3> path) {
        for (int i = 0; i < path.Count; i++) {
		    Vector3 C = path[i];
		    Vector3 D = path[(i + 1) % path.Count];
			Vector3? intersection = SegmentMath.SegmentSegmentIntersection(A, B, C, D);
			if (intersection != null) {
				return intersection;
			}
		}
        return null;
    }

    public static Vector3? SegmentSegmentIntersection(Vector3 A, Vector3 B, Vector3 C, Vector3 D) {
        float x1 = A.x;
        float y1 = A.z;
        float x2 = B.x;
        float y2 = B.z;
        float x3 = C.x;
        float y3 = C.z;
        float x4 = D.x;
        float y4 = D.z;

        float det = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

		if (det == 0) {
			return null;
		}
		
		float x = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
		x = x / det;
		float y = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);
		y = y / det;
		Vector3 I = new Vector3(x, 0, y);

		if (Vector3.Dot(B - I, B - A) < 0f) {
			return null;
		}
		if (Vector3.Dot(A - I, A - B) < 0f) {
			return null;
		}
		if (Vector3.Dot(D - I, D - C) < 0f) {
			return null;
		}
		if (Vector3.Dot(C - I, C - D) < 0f) {
			return null;
		}

        return I;
    }
}