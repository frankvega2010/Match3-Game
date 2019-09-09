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
    public int width;
    public int height;

   /* public Vector2 GetPosition(int column, int row)
    {
        Vector2 position;

        position = gridPositions[column, row];

        return position;
    }

    public Colors GetColor(int column, int row)
    {
        Colors color;

        color = gridColors[column, row];

        return color;
    }*/

    
}
