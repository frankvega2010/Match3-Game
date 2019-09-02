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

    private Colors[,] gridColors;
    private Vector2[,] gridPositions;
    private int cellPosition;
    private int width;
    private int height;

    public void InitializeGrid(int newHeight,int newWidth,int increasedPosition)
    {
        cellPosition = 0;
        width = newWidth;
        height = newHeight;

        gridColors = new Colors[height, width];
        gridPositions = new Vector2[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridColors[i,j] = (Colors)Random.Range(1, 4);
                gridPositions[i, j] = new Vector2(increasedPosition * j, increasedPosition * i);
            }
        }

        CheckRepeatedColorsVertical();
    }

    public void ShowGridData()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Debug.Log("Position " + i + " " + j + " = " + "Color : " + gridColors[i, j] + " / Position in Matrix = " + gridPositions[i, j]);
            }
        }
    }

    public Vector2 GetPosition(int column, int row)
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
    }

    public void CheckRepeatedColorsVertical()
    {
        int findSameColor = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(i != 0 && i < height - 1)
                {
                    findSameColor++;

                    if (gridColors[i, j] == gridColors[i-1, j])
                    {
                        findSameColor++;
                    }

                    if (gridColors[i, j] == gridColors[i + 1, j])
                    {
                        findSameColor++;
                    }

                    if(findSameColor >= 3)
                    {
                        gridColors[i, j] = Colors.blank;
                    }

                    findSameColor = 0;
                }
            }
        }
    }
}
