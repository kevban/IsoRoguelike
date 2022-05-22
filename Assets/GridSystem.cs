using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private int[,] gridArray;
    public GridSystem(int width, int height) { 
        this.width = width;
        this.height = height;
        gridArray = new int[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++) {
            for (int s = 0; s < gridArray.GetLength(1); s++) { 
                
            }
        }
    }
}
