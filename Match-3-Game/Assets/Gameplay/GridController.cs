using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2[] swap;
    public Vector2[] tiles = new Vector2[2];
    public List<GameObject> blocks;
    public GridView gridView;
    private GridModel grid;
    public bool[] swapTiles;


    // Start is called before the first frame update
    void Start()
    {
        grid = new GridModel();
        InitializeGrid(9,9,2);
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

        for (int i = 0; i < 5; i++)
        {
            CheckRepeatedColorsVertical();
            CheckRepeatedColorsHorizontal();
        }
        
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
                        grid.gridColors[i - 1, j] = RandomColor();
                        grid.gridColors[i, j] = RandomColor();
                        grid.gridColors[i + 1, j] = RandomColor();
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
                        grid.gridColors[i, j - 1] = RandomColor();
                        grid.gridColors[i, j] = RandomColor();
                        grid.gridColors[i, j + 1] = RandomColor();
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

    private void DrawGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                blocks.Add(gridView.DrawGrid(grid.gridColors[i, j], grid.gridPositions[i, j], "Block: ", i, j));
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
            for (int i = 0; i < grid.height; i++)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    if(swap[w] == grid.gridPositions[i,j])
                    {
                        tiles[w].x = i;
                        tiles[w].y = j;
                    }
                }
            }
        }

        if (CheckSwap())
        {
            
            auxColor = grid.gridColors[(int)tiles[0].x, (int)tiles[0].y];

            grid.gridColors[(int)tiles[0].x, (int)tiles[0].y] = grid.gridColors[(int)tiles[1].x, (int)tiles[1].y];
            grid.gridColors[(int)tiles[1].x, (int)tiles[1].y] = auxColor;

            for (int i = 0; i < swap.Length; i++)
            {
                for (int j = 0; j < blocks.Count; j++)
                {
                    auxPos = new Vector2(blocks[j].transform.position.x, blocks[j].transform.position.y);
                    if (swap[i] == auxPos)
                    {
                        gridView.ChangeColor(blocks[j], grid.gridColors[(int)tiles[i].x, (int)tiles[i].y]);
                    }
                }
            }
        }
    }

    private bool CheckSwap()
    {
        /*Vector2[] pos = new Vector2[4];
        pos[0] = grid.gridPositions[(int)tiles[0].x, (int)tiles[0].y - 1];
        pos[1] = grid.gridPositions[(int)tiles[0].x, (int)tiles[0].y + 1];
        pos[2] = grid.gridPositions[(int)tiles[0].x - 1, (int)tiles[0].y];
        pos[3] = grid.gridPositions[(int)tiles[0].x + 1, (int)tiles[0].y];*/


        /*if(tiles[1].x != 0)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == pos[0])
            {
                return true;
            }
        }

        if(tiles[1].x != 0)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == pos[0])
            {
                return true;
            }
        }*/


        /*for (int i = 0; i < pos.Length; i++)
        {
            if(pos[i].x != 0 && pos[i].x != )
            {

            }
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == pos[i])
            {
                return true;
            }
        }*/

        if((int)tiles[0].y != 0)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == grid.gridPositions[(int)tiles[0].x, (int)tiles[0].y - 1])
            {
                return true;
            }
        }

        if ((int)tiles[0].y != 16)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == grid.gridPositions[(int)tiles[0].x, (int)tiles[0].y + 1])
            {
                return true;
            }
        }

        if ((int)tiles[0].x != 0)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == grid.gridPositions[(int)tiles[0].x - 1, (int)tiles[0].y])
            {
                return true;
            }
        }

        if ((int)tiles[0].x != 16)
        {
            if (grid.gridPositions[(int)tiles[1].x, (int)tiles[1].y] == grid.gridPositions[(int)tiles[0].x + 1, (int)tiles[0].y])
            {
                return true;
            }
        }

        return false;
    }
}
