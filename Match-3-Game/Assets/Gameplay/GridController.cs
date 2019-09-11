using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int rows;
    public int columns;
    public Vector2[] swap;
    public Vector2[] tiles = new Vector2[2];
    public List<GameObject> blocks;
    public GridView gridView;
    private GridModel grid;
    public bool[] swapTiles;

    //
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridModel();
        InitializeGrid(rows, columns, 2);
        ShowGridData();
        DrawGrid();
    }

    /*// Update is called once per frame
    void Update()
    {
        //CheckRepeatedColorsVertical();
        //CheckRepeatedColorsHorizontal();
        //Debug.Log("asd");
        //DrawGrid();
    }*/

    public void InitializeGrid(int newRows, int newColumns, int increasedPosition)
    {
        grid.cellPosition = 0;
        grid.columns = newColumns;
        grid.rows = newRows;

        grid.gridColors = new GridModel.Colors[grid.rows, grid.columns];
        grid.gridPositions = new Vector2[grid.rows, grid.columns];

        for (int i = 0; i < grid.rows; i++)
        {
            for (int j = 0; j < grid.columns; j++)
            {
                grid.gridColors[i, j] = RandomColor();
                grid.gridPositions[i, j] = new Vector2(increasedPosition * j, increasedPosition * i);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            CheckRepeatedColorsVertical();
            CheckRepeatedColorsHorizontal();
        }
        
    }

    public void CheckRepeatedColorsVertical()
    {
        int findSameColor = 0;

        for (int r = 0; r < grid.rows; r++)
        {
            for (int c = 0; c < grid.columns; c++)
            {
                if (r != 0 && r < grid.rows - 1)
                {
                    findSameColor++;

                    if (grid.gridColors[r, c] == grid.gridColors[r - 1, c])
                    {
                        findSameColor++;
                    }

                    if (grid.gridColors[r, c] == grid.gridColors[r + 1, c])
                    {
                        findSameColor++;
                    }

                    if (findSameColor >= 3)
                    {
                        //grid.gridColors[i, j] = GridModel.Colors.blank;
                        grid.gridColors[r - 1, c] = RandomColor();
                        grid.gridColors[r, c] = RandomColor();
                        grid.gridColors[r + 1, c] = RandomColor();
                    }

                    findSameColor = 0;
                }
            }
        }
    }

    public void CheckRepeatedColorsHorizontal()
    {
        int findSameColor = 0;

        for (int r = 0; r < grid.rows; r++)
        {
            for (int c = 0; c < grid.columns; c++)
            {
                if (c != 0 && c < grid.columns - 1)
                {
                    findSameColor++;

                    if (grid.gridColors[r, c] == grid.gridColors[r, c - 1])
                    {
                        findSameColor++;
                    }

                    if (grid.gridColors[r, c] == grid.gridColors[r, c + 1])
                    {
                        findSameColor++;
                    }

                    if (findSameColor >= 3)
                    {
                        //grid.gridColors[i, j] = GridModel.Colors.blank;
                        grid.gridColors[r, c - 1] = RandomColor();
                        grid.gridColors[r, c] = RandomColor();
                        grid.gridColors[r, c + 1] = RandomColor();
                    }

                    findSameColor = 0;
                }
            }
        }
    }

    public void ShowGridData()
    {
        for (int r = 0; r < grid.rows; r++)
        {
            for (int c = 0; c < grid.columns; c++)
            {
                Debug.Log("Position " + r + " " + c + " = " + "Color : " + grid.gridColors[r, c] + " / Position in Matrix = " + grid.gridPositions[r, c]);
            }
        }
    }

    private GridModel.Colors RandomColor()
    {
        return (GridModel.Colors)Random.Range(1, 4);
    }

    private void DrawGrid()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                blocks.Add(gridView.DrawGrid(grid.gridColors[r, c], grid.gridPositions[r, c], "Block: ", r, c));
            }
        }
    }

    public void AddSwapTile(Vector2 newTile)
    {
        if (swapTiles[0])
        {
            swap[1] = newTile;
            SwapTile();
            swapTiles[0] = false;
        }
        else
        {
            swap[0] = newTile;
            swapTiles[0] = true;
        }
    }

    private void SwapTile()
    {
        Vector2 auxPos;
        GridModel.Colors auxColor;

        for (int w = 0; w < swap.Length; w++)
        {
            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    if(swap[w] == grid.gridPositions[r,c])
                    {
                        tiles[w].y = r;
                        tiles[w].x = c;
                    }
                }
            }
        }

        if (CheckSwap())
        {
            
            auxColor = grid.gridColors[(int)tiles[0].y, (int)tiles[0].x];

            grid.gridColors[(int)tiles[0].y, (int)tiles[0].x] = grid.gridColors[(int)tiles[1].y, (int)tiles[1].x];
            grid.gridColors[(int)tiles[1].y, (int)tiles[1].x] = auxColor;

            for (int i = 0; i < swap.Length; i++)
            {
                for (int j = 0; j < blocks.Count; j++)
                {
                    auxPos = new Vector2(blocks[j].transform.position.x, blocks[j].transform.position.y);
                    if (swap[i] == auxPos)
                    {
                        gridView.ChangeColor(blocks[j], grid.gridColors[(int)tiles[i].y, (int)tiles[i].x]);
                    }
                }
            }
        }
    }

    private bool CheckSwap()
    {

        if(tiles[0].y > 0)
        {
            if (grid.gridPositions[(int)tiles[1].y, (int)tiles[1].x] == grid.gridPositions[(int)tiles[0].y - 1, (int)tiles[0].x])
            {
                return true;
            }
        }

        if (tiles[0].x > 0)
        {
            if (grid.gridPositions[(int)tiles[1].y, (int)tiles[1].x] == grid.gridPositions[(int)tiles[0].y, (int)tiles[0].x - 1])
            {
                return true;
            }
        }

        if(tiles[0].y < rows - 1)
        {
            if (grid.gridPositions[(int)tiles[1].y, (int)tiles[1].x] == grid.gridPositions[(int)tiles[0].y + 1, (int)tiles[0].x])
            {
                return true;
            }
        }

        if (tiles[0].x < rows - 1)
        {
            if (grid.gridPositions[(int)tiles[1].y, (int)tiles[1].x] == grid.gridPositions[(int)tiles[0].y, (int)tiles[0].x + 1])
            {
                return true;
            }
        }

        return false;
    }
}
