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
    public Vector2[] tilesGridPosition = new Vector2[2];
    public GameObject[,] gridTiles = new GameObject[9, 9];
    public GameObject[] tilesToSwap;
    public List<GameObject> tilesToDelete;
    public List<GameObject> sameColorTilesFound;

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

    // Update is called once per frame
    void Update()
    {

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
                    //block.GetComponent<SpriteRenderer>().color = Color.white;
                    gridView.ChangeColor(block, GridModel.Colors.blank);
                }
            }

            tilesToDelete.Clear();
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

    private void CheckLine(GameObject block, directions raycastDirection)
    {
        SpriteRenderer blockSprite = block.GetComponent<SpriteRenderer>();
        RaycastHit2D hit = Physics2D.Raycast(block.transform.position, Vector3.up, 0.2f);
        CheckDirection(ref hit, block, raycastDirection);
        Debug.Log("searching for hit");

        for (int i = 0; i < 8; i++)
        {
            if (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().color == blockSprite.color)
            {
                Debug.Log("hit!");
                sameColorTilesFound.Add(hit.collider.gameObject);
                CheckDirection(ref hit, hit.collider.gameObject, raycastDirection);
            }

            sameColorTilesFound = sameColorTilesFound.Distinct().ToList();
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
                CheckLine(gridTiles[(int)tilesGridPosition[i].y, (int)tilesGridPosition[i].x], rayDirections[c]);
            }

            sameColorTilesFound = sameColorTilesFound.Distinct().ToList();

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

    private void CheckDirection(ref RaycastHit2D hit,GameObject target, directions raycastDirection)
    {
        switch (raycastDirection)
        {
            case directions.up:
                hit = Physics2D.Raycast(target.transform.position + Vector3.up * 1.2f, Vector3.up, 0.5f);
                break;
            case directions.down:
                hit = Physics2D.Raycast(target.transform.position + Vector3.up * -1.2f, Vector3.up * -1, 0.5f);
                break;
            case directions.left:
                hit = Physics2D.Raycast(target.transform.position + Vector3.right * -1.2f, Vector3.right * -1, 0.5f);
                break;
            case directions.right:
                hit = Physics2D.Raycast(target.transform.position + Vector3.right * 1.2f, Vector3.right, 0.5f);
                break;
            default:
                break;
        }
    }
}