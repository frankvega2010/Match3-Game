using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    public GameObject template;
    public Transform newParent;
   /* // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public GameObject DrawGrid(GridModel.Colors tileColor,Vector2 tilePosition, string newName, int i, int j)
    {
        GameObject newTile = Instantiate(template, new Vector3(tilePosition.x,tilePosition.y),Quaternion.identity);
        //newTile.transform.position = new Vector3(tilePosition.x, tilePosition.y, 0);
        newTile.SetActive(true);
        newTile.name = newName + i + " - " + j;

        //newTile.GetComponent<SpriteRenderer>().color = Color.red;

        switch (tileColor)
        {
            case GridModel.Colors.blank:
                newTile.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case GridModel.Colors.red:
                newTile.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case GridModel.Colors.blue:
                newTile.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case GridModel.Colors.green:
                newTile.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }

        return newTile;
    }

    public void ChangeColor(GameObject block, GridModel.Colors color)
    {
        switch (color)
        {
            case GridModel.Colors.blank:
                block.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case GridModel.Colors.red:
                block.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case GridModel.Colors.blue:
                block.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case GridModel.Colors.green:
                block.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }
    }
}
