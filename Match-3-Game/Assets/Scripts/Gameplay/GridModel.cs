using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModel
{
    public enum Colors
    {
        blank,
        red,
        blue,
        green,
        allColors
    }

    public Colors[,] gridColors;
    public Vector2[,] gridPositions;
    public int cellPosition;
    public int columns;
    public int rows;   
}
