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

    public void DrawGrid(GridModel.Colors tileColor,Vector2 tilePosition)
    {
        GameObject newTile = Instantiate(template, new Vector3(tilePosition.x,tilePosition.y),Quaternion.identity);
        //newTile.transform.position = new Vector3(tilePosition.x, tilePosition.y, 0);
        newTile.SetActive(true);

        //newTile.GetComponent<SpriteRenderer>().color = Color.red;

        switch (tileColor)
        {
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
    }
}
