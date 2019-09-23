using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridController : MonoBehaviour
{
    enum directions
    {
        up,
        down,
        left,
        right,
        allDirs
    }

    public int rows;
    public int columns;
    public GameObject template;
    public LayerMask masks;
    public Vector2[] tilesGridPosition = new Vector2[2];
    public GameObject[,] gridTiles = new GameObject[9, 9];
    public GameObject[] tilesToSwap;
    public GameObject lastSameColor;
    public int lastSameColorRow;
    public int lastSameColorColumn;
    public List<GameObject> tilesToDelete;
    public List<GameObject> sameColorTilesFound;

    private Vector2[] nullTilesPosition;
    public GridView gridView;
    private GridModel grid;
    public float distance = 0;
    private directions[] rayDirections = new directions[4];

    // Start is called before the first frame update
    void Start()
    {
        rayDirections[0] = directions.up;
        rayDirections[1] = directions.down;
        rayDirections[2] = directions.left;
        rayDirections[3] = directions.right;
        grid = new GridModel();
        InitializeGrid(rows, columns, 2);
        ShowGridData();
        DrawGrid();
    }

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
                grid.gridPositions[r, c] = new Vector2(increasedPosition * c, increasedPosition * r * -1);
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

    private void SwapTile()
    {
        for (int w = 0; w < tilesToSwap.Length; w++)
        {
            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    if (tilesToSwap[w].transform.position == gridTiles[r, c].transform.position)
                    {
                        tilesGridPosition[w].y = r;
                        tilesGridPosition[w].x = c;
                    }
                }
            }
        }

        if (CheckSwap())
        {
            Vector3 auxTilePosition = tilesToSwap[0].transform.position;
            tilesToSwap[0].transform.position = tilesToSwap[1].transform.position;
            tilesToSwap[1].transform.position = auxTilePosition;

            GameObject auxGridTile = gridTiles[(int)tilesGridPosition[0].y, (int)tilesGridPosition[0].x];
            gridTiles[(int)tilesGridPosition[0].y, (int)tilesGridPosition[0].x] = tilesToSwap[1];
            gridTiles[(int)tilesGridPosition[1].y, (int)tilesGridPosition[1].x] = auxGridTile;

            GridModel.Colors auxColorTile = grid.gridColors[(int)tilesGridPosition[0].y, (int)tilesGridPosition[0].x];
            grid.gridColors[(int)tilesGridPosition[0].y, (int)tilesGridPosition[0].x] = grid.gridColors[(int)tilesGridPosition[1].y, (int)tilesGridPosition[1].x];
            grid.gridColors[(int)tilesGridPosition[1].y, (int)tilesGridPosition[1].x] = auxColorTile;

            CheckMatch3(0, rayDirections.Length - 2);
            CheckMatch3(rayDirections.Length - 2, rayDirections.Length);

            if (tilesToDelete.Count >= 3)
            {
                Debug.Log("Deleting blocks..");
                foreach (GameObject block in tilesToDelete)
                {
                    for (int r = 0; r < grid.rows; r++)
                    {
                        for (int c = 0; c < grid.columns; c++)
                        {
                            if (block == gridTiles[r, c])
                            {
                                Destroy(gridTiles[r, c]);
                                gridTiles[r, c] = null;
                            }
                        }
                    }
                }
            }

            tilesToDelete.Clear();
            RefillGrid();
            /*for (int i = 0; i < 8; i++)
            {
                if(RefillTilesAfter())
                {
                    i = 8;
                }
                //RefillTilesAfter();
            }*/
            
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

            if (tilesGridPosition[1] == new Vector2(tilesGridPosition[0].x - 1, tilesGridPosition[0].y))
            {
                return true;
            }
        }

        if (tilesGridPosition[0].y < rows - 1)
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

    private void CheckLine(int positionY, int positionX , directions raycastDirection)
    {
        GameObject block = gridTiles[positionY, positionX];
        GridModel.Colors blockColor = grid.gridColors[positionY, positionX];
        GameObject nextBlock = new GameObject();
        int newPositionX = 0;
        int newPositionY = 0;
        //SpriteRenderer blockSprite = block.GetComponent<SpriteRenderer>();
        Debug.Log("searching for hit");

        for (int i = 0; i < 8; i++)
        {
            CheckDirection(ref nextBlock, raycastDirection, i, positionX, positionY,ref newPositionX,ref newPositionY);

            if (grid.gridColors[newPositionY, newPositionX] == blockColor) // TERMINAR
            {
                Debug.Log("hit!");
                sameColorTilesFound.Add(nextBlock);
                lastSameColor = nextBlock;
                lastSameColorColumn = newPositionX;
                lastSameColorRow = newPositionY;
            }
            else
            {
                i = 8;
            }
        }

        sameColorTilesFound.Add(block);
        sameColorTilesFound = sameColorTilesFound.Distinct().ToList();
    }

    private void CheckMatch3(int startIndex, int lastIndex)
    {
        for (int i = 0; i < tilesGridPosition.Length; i++)
        {
            Debug.Log("Checking Match3");

            for (int c = startIndex; c < lastIndex; c++)
            {
                Debug.Log("Enter for loop");
                CheckLine((int)tilesGridPosition[i].y, (int)tilesGridPosition[i].x, rayDirections[c]);
            }

            //sameColorTilesFound = sameColorTilesFound.Distinct().ToList();

            foreach (GameObject item in sameColorTilesFound)
            {
                Debug.Log("Found: " + item.name);
            }

            if (sameColorTilesFound.Count >= 3)
            {
                foreach (GameObject tile in sameColorTilesFound)
                {
                    tilesToDelete.Add(tile);
                }
            }

            sameColorTilesFound.Clear();
        }
    }

    private void ClearAllMatches()
    {

    }
 
    private void CheckMatch3(int startIndex, int lastIndex, int rows, int columns,bool isVertical)
    {
        bool foundSameTile = false;
       // GameObject lastSameColor = new GameObject();

        if (isVertical)
        {
            for (int c = 0; c < columns; c++)
            {
                for (int r = rows - 1; r >= 0; r--)
                {
                    CheckLine(r, c, rayDirections[0]);

                    if (sameColorTilesFound.Count >= 3)
                    {
                        foreach (GameObject tile in sameColorTilesFound)
                        {
                            tilesToDelete.Add(tile);
                        }
                    }

                    sameColorTilesFound.Clear();

                    Debug.Log(gridTiles[lastSameColorRow, c].name);
                    r = lastSameColorRow;
                }
            }
        }
        else
        {
            for (int r = rows - 1; r >= 0; r--)
            {
                for (int c = 0; c < columns; c++)
                {
                    CheckLine(r, c, rayDirections[3]);

                    if (sameColorTilesFound.Count >= 3)
                    {
                        foreach (GameObject tile in sameColorTilesFound)
                        {
                            tilesToDelete.Add(tile);
                        }
                    }

                    sameColorTilesFound.Clear();

                    Debug.Log(gridTiles[r, lastSameColorColumn].name);
                    c = lastSameColorColumn;
                }
            }
        }
        /*if(isVertical)
        {
            for (int v = 0; v < columns; v++) // v = c
            {
                for (int t = 0; t < rows; t++) // t = r
                {
                    Debug.Log("Checking Match3");

                    foreach (GameObject tile in sameColorTilesFound)
                    {
                        if (tile == gridTiles[t, v])
                        {
                            foundSameTile = true;
                        }

                        foreach (GameObject deletedTile in tilesToDelete)
                        {
                            if (deletedTile == gridTiles[t, v])
                            {
                                foundSameTile = true;
                            }
                        }
                    }

                    if (!foundSameTile)
                    {
                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            Debug.Log("Enter for loop");

                            if (!foundSameTile)
                            {
                                CheckLine(t, v, rayDirections[i]);
                            }
                        }
                    }

                    foundSameTile = false;

                    foreach (GameObject item in sameColorTilesFound)
                    {
                        Debug.Log("Found: " + item.name);
                    }

                    if (sameColorTilesFound.Count >= 3)
                    {
                        foreach (GameObject tile in sameColorTilesFound)
                        {
                            tilesToDelete.Add(tile);
                        }
                    }

                    sameColorTilesFound.Clear();
                }
            }
        }
        else
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Debug.Log("Checking Match3");

                    foreach (GameObject tile in sameColorTilesFound)
                    {
                        if (tile == gridTiles[r, c])
                        {
                            foundSameTile = true;
                        }

                        foreach (GameObject deletedTile in tilesToDelete)
                        {
                            if (deletedTile == gridTiles[r, c])
                            {
                                foundSameTile = true;
                            }
                        }
                    }

                    if (!foundSameTile)
                    {
                        for (int i = startIndex; i < lastIndex; i++)
                        {
                            Debug.Log("Enter for loop");

                            if (!foundSameTile)
                            {
                                CheckLine(r, c, rayDirections[i]);
                            }
                        }
                    }

                    foundSameTile = false;

                    foreach (GameObject item in sameColorTilesFound)
                    {
                        Debug.Log("Found: " + item.name);
                    }

                    if (sameColorTilesFound.Count >= 3)
                    {
                        foreach (GameObject tile in sameColorTilesFound)
                        {
                            tilesToDelete.Add(tile);
                        }
                    }

                    sameColorTilesFound.Clear();
                }
            }
        }*/


    }

    private void CheckDirection(ref GameObject target, directions raycastDirection, int amount, int X,int Y, ref int newPosX,ref int newPosY)
    {
        int newY = 0;
        int newX = 0;

        switch (raycastDirection)
        {
            case directions.up:
                newY = Y - amount;
                if (newY >= 0 && newY < rows)
                {
                    target = gridTiles[newY, X];
                    newPosX = X;
                    newPosY = newY;
                }
                break;
            case directions.down:
                newY = Y + amount;
                if (newY >= 0 && newY < rows)
                {
                    target = gridTiles[newY, X];
                    newPosX = X;
                    newPosY = newY;
                }
                break;
            case directions.left:
                newX = X - amount;
                if (newX >= 0 && newX < columns)
                {
                    target = gridTiles[Y , newX];
                    newPosX = newX;
                    newPosY = Y;
                }
                break;
            case directions.right:
                newX = X + amount;
                if (newX >= 0 && newX < columns)
                {
                    target = gridTiles[Y, newX];
                    newPosX = newX;
                    newPosY = Y;
                }
                break;
            default:
                break;
        }
    }

    private void RefillGrid()
    {
        for (int i = 0; i < 3; i++)
        {
            int nullCount = 0;

            Debug.Log("Searching for null tiles");

            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    if (gridTiles[r, c] == null)
                    {
                        nullCount++;
                    }
                }
            }

            Debug.Log("Null Tiles : " + nullCount);

            nullTilesPosition = new Vector2[nullCount];

            Debug.Log("Assigning the position of the null tiles");

            int currentNull = 0;

            for (int r = 0; r < grid.rows; r++)
            {
                for (int c = 0; c < grid.columns; c++)
                {
                    if (gridTiles[r, c] == null)
                    {
                        nullTilesPosition[currentNull] = new Vector2(c, r);
                        currentNull++;
                    }
                }
            }

            Debug.Log("Applying the correct position to the nearest tiles");

            ReplaceTiles();
        }
    }

    private bool RefillTilesAfter()
    {
        bool isAllCleared = false;

        CheckMatch3(rayDirections.Length - 2, rayDirections.Length, rows, columns, false);
        CheckMatch3(rayDirections.Length - 2, rayDirections.Length, rows, columns, true);

        if (tilesToDelete.Count >= 3)
        {
            isAllCleared = false;
            Debug.Log("Deleting blocks..");
            foreach (GameObject block in tilesToDelete)
            {
                for (int r = 0; r < grid.rows; r++)
                {
                    for (int c = 0; c < grid.columns; c++)
                    {
                        if (block == gridTiles[r, c])
                        {
                            Destroy(gridTiles[r, c]);
                            gridTiles[r, c] = null;
                        }
                    }
                }
            }

            RefillGrid();
        }
        else
        {
            isAllCleared = true;
        }

        tilesToDelete.Clear();

        if(isAllCleared)
        {
            return true;
        }

        return false;
    }

    private void ReplaceTiles()
    {
        GameObject blockToBring;

        for (int n = nullTilesPosition.Length-1; n >= 0; n--)
        {
            for (int yNull = 8; yNull >= 0; yNull--)
            {
                if(gridTiles[yNull, (int)nullTilesPosition[n].x] == null)
                {
                    for (int yFill = 0; yFill < 9; yFill++)
                    {
                        int newPositionX = (int)nullTilesPosition[n].x;
                        int newPositionY = yNull - yFill;

                        if (newPositionY >= 0 && newPositionY < rows)
                        {
                            Debug.Log("newPositionY : " + newPositionY);
                            Debug.Log("newPositionX : " + newPositionX);

                            if (gridTiles[newPositionY, newPositionX] != null)
                            {
                                blockToBring = gridTiles[newPositionY, newPositionX];
                                gridTiles[newPositionY, newPositionX] = null;
                                blockToBring.transform.position = grid.gridPositions[yNull, (int)nullTilesPosition[n].x];
                                gridTiles[yNull, (int)nullTilesPosition[n].x] = blockToBring;
                                yFill = 9;
                            }
                        }


                    }
                }
                
            }
        }

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (gridTiles[r, c] == null)
                {
                    grid.gridColors[r, c] = RandomColor();
                    gridTiles[r, c] = gridView.DrawGrid(grid.gridColors[r, c], grid.gridPositions[r, c], "Block: ", r, c);
                }
            }
        }

        
    }
}