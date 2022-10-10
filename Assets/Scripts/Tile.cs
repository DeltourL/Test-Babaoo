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

    public void Initialize(Vector2 position, Vector2Int coords, Shader shader, Texture texture, int boardSize)
    {
        // position in world
        transform.position = position;
        lastCorrectPosition = position;

        // coordinates on the board
        coordinates = coords;
        correctCoordinates = coords;

        // setting up the puzzle image on the tile
        GetComponent<Renderer>().material = new Material(shader)
        {
            mainTexture = texture,
            mainTextureOffset = new Vector2(1.0f / boardSize * coords.x, 1.0f / boardSize * coords.y),
            mainTextureScale = new Vector2(1.0f / boardSize, 1.0f / boardSize)
        };
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
