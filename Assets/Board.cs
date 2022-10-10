using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private readonly int boardSize = 3;

    public void CreateBoard()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // first tile position in bottom left, centered on 0
                Vector2 position = new Vector2((boardSize - 1) * -1 * 0.5f, (boardSize - 1) * -1 * 0.5f);

                position += new Vector2(x, y);

                // tile creation
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.transform.position = position;
                tile.transform.parent = transform;

                tile.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}
