using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FenceDetection {

    public static List<Vector3> PathFromGround(Ground ground) {
        List<Vector3> path = new List<Vector3>();

        int[][] groundGrid = new int[ground.width][];
        for (int i = 0; i < ground.width; i++) {
            groundGrid[i] = new int[ground.height];
            for (int j = 0; j < ground.height; j++) {
                groundGrid[i][j] = ground.grid[i + j * ground.width];
            }
        }

        int cursorI = 0;
        int cursorJ = 0;
        path.Add(FenceDetection.SetFirst(groundGrid, ground.width, ground.height, ref cursorI, ref cursorJ));
        while (NextLeft(groundGrid, ground.width, ground.height, ref cursorI, ref cursorJ)) {
            Debug.Log("Next Left");
            path.Add(new Vector3(cursorI, 0, cursorJ));
        }
        while (NextTop(groundGrid, ground.width, ground.height, ref cursorI, ref cursorJ)) {
            Debug.Log("Next Top");
            path.Add(new Vector3(cursorI, 0, cursorJ));
        }
        while (NextRight(groundGrid, ground.width, ground.height, ref cursorI, ref cursorJ)) {
            Debug.Log("Next Right");
            path.Add(new Vector3(cursorI, 0, cursorJ));
        }
        while (NextBottom(groundGrid, ground.width, ground.height, ref cursorI, ref cursorJ)) {
            Debug.Log("Next Bottom");
            path.Add(new Vector3(cursorI, 0, cursorJ));
        }
        path.RemoveAt(path.Count - 1);

        return path;
    }

    private static Vector3 SetFirst(int[][] groundGrid, int width, int height, ref int cursorI, ref int cursorJ) {
        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {
                if (groundGrid[i][j] == 0) {
                    Debug.Log("FenceDetection first point found " + i + " " + j);
                    cursorI = i;
                    cursorJ = j;
                    return new Vector3(i, 0, j);
                }
            }
        }
        Debug.LogError("FenceDetection error, cannot find first point");
        return new Vector3(- 1, 0, - 1);
    }

    private static bool NextLeft(int[][] groundGrid, int width, int height, ref int cursorI, ref int cursorJ) {
        if (groundGrid[cursorI - 1][cursorJ] == 0) {
            cursorI--;
        }
        else if (groundGrid[cursorI][cursorJ + 1] == 0) {
            cursorJ++;
        }
        else if (groundGrid[cursorI + 1][cursorJ] == 0) {
            cursorI++;
        }
        else {
            return false;
        }
        groundGrid[cursorI][cursorJ] = 1;
        return true;
    }

    private static bool NextTop(int[][] groundGrid, int width, int height, ref int cursorI, ref int cursorJ) {
        if (groundGrid[cursorI][cursorJ + 1] == 0) {
            cursorJ++;
        }
        else if (groundGrid[cursorI + 1][cursorJ] == 0) {
            cursorI++;
        }
        else if (groundGrid[cursorI][cursorJ - 1] == 0) {
            cursorJ--;
        }
        else {
            return false;
        }
        groundGrid[cursorI][cursorJ] = 1;
        return true;
    }

    private static bool NextRight(int[][] groundGrid, int width, int height, ref int cursorI, ref int cursorJ) {
        if (groundGrid[cursorI + 1][cursorJ] == 0) {
            cursorI++;
        }
        else if (groundGrid[cursorI][cursorJ - 1] == 0) {
            cursorJ--;
        }
        else if (groundGrid[cursorI - 1][cursorJ] == 0) {
            cursorI--;
        }
        else {
            return false;
        }
        groundGrid[cursorI][cursorJ] = 1;
        return true;
    }

    private static bool NextBottom(int[][] groundGrid, int width, int height, ref int cursorI, ref int cursorJ) {
        if (groundGrid[cursorI][cursorJ - 1] == 0) {
            cursorJ--;
        }
        else if (groundGrid[cursorI - 1][cursorJ] == 0) {
            cursorI--;
        }
        else if (groundGrid[cursorI][cursorJ + 1] == 0) {
            cursorJ++;
        }
        else {
            return false;
        }
        groundGrid[cursorI][cursorJ] = 1;
        return true;
    }
}