using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    public Vector2Int coordinates;
    public Vector2Int correctCoordinates;

    public Vector3 lastCorrectPosition;
    public event Action<Tile> OnTileMoved;

    private void Start()
    {
        lastCorrectPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        // find pointer position in world
        Vector3 mouseToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // move to foreground
        mouseToWorldPosition.z = -1f;
        // make the tile follow the pointer
        transform.position = mouseToWorldPosition;
    }

    private void OnMouseUp()
    {
        // if released above empty space
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Empty Space")))
        {
            OnTileMoved?.Invoke(this);
        }
        else
        {
            // reset position
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = lastCorrectPosition;
    }

}
