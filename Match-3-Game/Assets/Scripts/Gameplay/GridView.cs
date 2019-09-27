using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    public GridController controller;
    public GameObject template;
    public Transform newParent;
    public Color replaceColor;
    public Text scoreText;


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.CheckBlockClicked(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.PreviewSwap(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (hit)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.CheckMouseRelease(hit.transform.gameObject);
            }
        }

#endif

        #if UNITY_ANDROID && !UNITY_EDITOR

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector3.zero);

            if (hit && touch.phase == TouchPhase.Began)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.CheckBlockClicked(hit.transform.gameObject);
            }

            if (hit && touch.phase == TouchPhase.Moved)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.PreviewSwap(hit.transform.gameObject);
            }

            if (hit && touch.phase == TouchPhase.Ended)
            {
                //Debug.Log(hit.transform.gameObject.name);
                controller.CheckMouseRelease(hit.transform.gameObject);
            }
        }

        #endif

    }

    public GameObject DrawGrid(GridModel.Colors tileColor,Vector2 tilePosition, string newName, int i, int j)
    {
        GameObject newTile = Instantiate(template, new Vector3(tilePosition.x,tilePosition.y),Quaternion.identity);
        newTile.SetActive(true);
        newTile.name = newName + i + " - " + j;

        switch (tileColor)
        {
            case GridModel.Colors.blank:
                newTile.GetComponent<SpriteRenderer>().color += replaceColor;
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
                block.GetComponent<SpriteRenderer>().color += replaceColor;
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
