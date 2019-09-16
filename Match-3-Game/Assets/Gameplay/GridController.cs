using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int rows;
    public int columns;
    public Vector2[] swap;
    public Vector2[] tilesGridPosition = new Vector2[2];
    public List<GameObject> blocks;
    public GridView gridView;
    private GridModel grid;
    public GameObject[,] gridTiles = new GameObject[9,9];
    public bool[] swapTiles;
    public GameObject[] tilesToSwap;
    

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

        for (int r = 0; r < grid.rows; r++)
        {
            for (int c = 0; c < grid.columns; c++)
            {
                grid.gridColors[r, c] = RandomColor();
                grid.gridPositions[r, c] = new Vector2(increasedPosition * c , increasedPosition * r * -1);
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
                gridTiles[r, c] = gridView.DrawGrid(grid.gridColors[r, c], grid.gridPositions[r, c], "Block: ", r, c);
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
        for (int w = 0; w < swap.Length; w++)
        {
            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    if(tilesToSwap[w].transform.position == gridTiles[r,c].transform.position)
                    {
                        tilesGridPosition[w].y = r;
                        tilesGridPosition[w].x = c;
                    }
                }
            }
        }

        if(CheckSwap())
        {
            Vector3 auxTilePosition = tilesToSwap[0].transform.position;
            tilesToSwap[0].transform.position = tilesToSwap[1].transform.position;
            tilesToSwap[1].transform.position = auxTilePosition;
        }

        for (int i = 0; i < 2; i++)
        {
            Debug.Log("NEW Block " + tilesGridPosition[i].y + " - " + tilesGridPosition[i].x + " / Color : " + grid.gridColors[(int)tilesGridPosition[i].y, (int)tilesGridPosition[i].x]);
        }
    }

    private bool CheckSwap()
    {
        Debug.Log("0: - Column " + tilesGridPosition[0].x + " / - Row: " + tilesGridPosition[0].y);
        Debug.Log("1: - Column " + tilesGridPosition[1].x + " / - Row: " + tilesGridPosition[1].y);

        Debug.Log(tilesGridPosition[0]);
        Debug.Log(tilesGridPosition[1]);

        if (tilesGridPosition[0].y > 0)
        {
            
            if (tilesGridPosition[1] == new Vector2(tilesGridPosition[0].x, tilesGridPosition[0].y - 1))
            {
                return true;
            }
        }

        if (tilesGridPosition[0].x > 0)
        {

            if (tilesGridPosition[1] == new Vector2(tilesGridPosition[0].x - 1, tilesGridPosition[0].y ))
            {
                return true;
            }
        }

        if(tilesGridPosition[0].y < rows - 1)
        {
            if (tilesGridPosition[1] == new Vector2(tilesGridPosition[0].x, tilesGridPosition[0].y + 1))
            {
                return true;
            }
        }

        if (tilesGridPosition[0].x < columns - 1)
        {
            if (tilesGridPosition[1] == new Vector2(tilesGridPosition[0].x + 1, tilesGridPosition[0].y))
            {
                return true;
            }
        }

        return false;
    }


    public void CheckBlockClicked(GameObject block)
    {
        if (tilesToSwap[0])
        {
            tilesToSwap[1] = block;
            SwapTile();
            tilesToSwap[0] = null;
            tilesToSwap[1] = null;
            tilesGridPosition[0] = Vector2.zero;
            tilesGridPosition[1] = Vector2.zero;
        }
        else
        {
            tilesToSwap[0] = block;
        }
    }
}