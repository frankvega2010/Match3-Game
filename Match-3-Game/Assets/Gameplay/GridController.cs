using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GridView gridView;
    private GridModel grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GridModel();
        InitializeGrid(9,9,2);
        ShowGridData();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                //gridView.DrawGrid(grid.GetColor(i,j),grid.GetPosition(i,j));
                gridView.DrawGrid(grid.gridColors[i,j], grid.gridPositions[i,j]);
                //gridColors[i, j] = (Colors)Random.Range(1, 4);
                //gridPositions[i, j] = new Vector2(increasedPosition * i, increasedPosition * j);
            }
        }
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void InitializeGrid(int newHeight, int newWidth, int increasedPosition)
    {
        grid.cellPosition = 0;
        grid.width = newWidth;
        grid.height = newHeight;

        grid.gridColors = new GridModel.Colors[grid.height, grid.width];
        grid.gridPositions = new Vector2[grid.height, grid.width];

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                grid.gridColors[i, j] = RandomColor();
                grid.gridPositions[i, j] = new Vector2(increasedPosition * j, increasedPosition * i);
            }
        }

        CheckRepeatedColorsVertical();
        CheckRepeatedColorsHorizontal();
    }

    public void CheckRepeatedColorsVertical()
    {
        int findSameColor = 0;

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                if (i != 0 && i < grid.height - 1)
                {
                    findSameColor++;

                    if (grid.gridColors[i, j] == grid.gridColors[i - 1, j])
                    {
                        findSameColor++;
                    }

                    if (grid.gridColors[i, j] == grid.gridColors[i + 1, j])
                    {
                        findSameColor++;
                    }

                    if (findSameColor >= 3)
                    {
                        //grid.gridColors[i, j] = GridModel.Colors.blank;
                        grid.gridColors[i, j] = RandomColor();
                    }

                    findSameColor = 0;
                }
            }
        }
    }

    public void CheckRepeatedColorsHorizontal()
    {
        int findSameColor = 0;

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                if (j != 0 && j < grid.width - 1)
                {
                    findSameColor++;

                    if (grid.gridColors[i, j] == grid.gridColors[i, j - 1])
                    {
                        findSameColor++;
                    }

                    if (grid.gridColors[i, j] == grid.gridColors[i, j + 1])
                    {
                        findSameColor++;
                    }

                    if (findSameColor >= 3)
                    {
                        //grid.gridColors[i, j] = GridModel.Colors.blank;
                        grid.gridColors[i, j] = RandomColor();
                    }

                    findSameColor = 0;
                }
            }
        }
    }

    public void ShowGridData()
    {
        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                Debug.Log("Position " + i + " " + j + " = " + "Color : " + grid.gridColors[i, j] + " / Position in Matrix = " + grid.gridPositions[i, j]);
            }
        }
    }

    private GridModel.Colors RandomColor()
    {
        return (GridModel.Colors)Random.Range(1, 4);
    }
}
