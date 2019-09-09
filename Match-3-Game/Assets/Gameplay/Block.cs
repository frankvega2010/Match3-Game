using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GridController controller;

    /*// Start is called before the first frame update
    void Start()
    {

    }*/

    private void OnMouseDown()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        controller.AddSwapTile(pos);
        Debug.Log(gameObject.name);
    }
}
