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
        grid.InitializeGrid(9,9,2);
        grid.ShowGridData();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                gridView.DrawGrid(grid.GetColor(i,j),grid.GetPosition(i,j));
                //gridColors[i, j] = (Colors)Random.Range(1, 4);
                //gridPositions[i, j] = new Vector2(increasedPosition * i, increasedPosition * j);
            }
        }
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
