using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private readonly int boardSize = 3;


    [SerializeField]
    private Texture puzzleImage;
    [SerializeField]
    private Shader shader;

    public void CreateBoard()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // first tile position in bottom left, full board is centered on 0
                Vector2 position = new Vector2((boardSize - 1) * -1 * 0.5f, (boardSize - 1) * -1 * 0.5f);

                position += new Vector2(x, y);

                // tile creation
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.transform.position = position;
                tile.transform.parent = transform;

                // setting up the image on the tile
                tile.GetComponent<Renderer>().material = new Material(shader)
                {
                    mainTexture = puzzleImage,
                    mainTextureOffset = new Vector2(1.0f / boardSize * x, 1.0f / boardSize * y),
                    mainTextureScale = new Vector2(1.0f / boardSize, 1.0f / boardSize)
                };

                // removing the middle piece
                if (x == Mathf.RoundToInt(boardSize / 2) && y == Mathf.RoundToInt(boardSize / 2))
                {
                    tile.SetActive(false);
                }
            }
        }
    }
}
