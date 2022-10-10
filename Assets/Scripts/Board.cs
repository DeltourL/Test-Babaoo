using System;
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

    // number of moves made to shuffle the board
    private int shuffleMoves = 20;
    private Vector2Int previousMove = Vector2Int.zero;
    private Tile emptySpace;

    private Tile[,] tiles;

    private void Start()
    {
        tiles = new Tile[boardSize, boardSize];
    }

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
                Tile tile = GameObject.CreatePrimitive(PrimitiveType.Quad).AddComponent<Tile>();
                tile.transform.position = position;
                tile.lastCorrectPostion = position;

                tile.transform.parent = transform;
                tile.coordinates = new Vector2Int(x, y);

                tile.OnTileMoved += MoveTile;

                // setting up the image on the tile
                tile.GetComponent<Renderer>().material = new Material(shader)
                {
                    mainTexture = puzzleImage,
                    mainTextureOffset = new Vector2(1.0f / boardSize * x, 1.0f / boardSize * y),
                    mainTextureScale = new Vector2(1.0f / boardSize, 1.0f / boardSize)
                };

                // removing the middle piece and saving it
                if (x == Mathf.RoundToInt(boardSize / 2) && y == Mathf.RoundToInt(boardSize / 2))
                {
                    emptySpace = tile;

                    // turn tile invisible
                    tile.GetComponent<MeshRenderer>().enabled = false;

                    // changing layer to check for legal moves when moving tiles
                    tile.gameObject.layer = LayerMask.NameToLayer("Empty Space");
                }

                tiles[x, y] = tile;
            }
        }
        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = 0; i < shuffleMoves; i++)
        {
            List<Vector2Int> possibleMoves = new List<Vector2Int>();

            // add every possible moves
            if (emptySpace.coordinates.x > 0)
            {
                possibleMoves.Add(new Vector2Int(-1, 0));
            }
            if (emptySpace.coordinates.x < boardSize - 1)
            {
                possibleMoves.Add(new Vector2Int(1, 0));
            }
            if (emptySpace.coordinates.y > 0)
            {
                possibleMoves.Add(new Vector2Int(0, -1));
            }
            if (emptySpace.coordinates.y < boardSize - 1)
            {
                possibleMoves.Add(new Vector2Int(0, 1));
            }

            // avoid back and forth movement during shuffle
            possibleMoves.Remove(previousMove * -1);

            // choose a random tile and move it
            Vector2Int move = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];
            Vector2Int chosenTileCoordinates = move + emptySpace.coordinates;
            MoveTile(tiles[chosenTileCoordinates.x, chosenTileCoordinates.y]);

            previousMove = move;
        }
    }

    // switch places between tile and empty space 
    void MoveTile(Tile tile)
    {
        //check if move is legal
        if ((tile.coordinates - emptySpace.coordinates).sqrMagnitude == 1)
        {
            Vector2Int targetCoord = emptySpace.coordinates;
            emptySpace.coordinates = tile.coordinates;
            tile.coordinates = targetCoord;

            tiles[emptySpace.coordinates.x, emptySpace.coordinates.y] = emptySpace;
            tiles[targetCoord.x, targetCoord.y] = tile;

            Vector3 targetPosition = emptySpace.transform.position;
            emptySpace.transform.position = tile.lastCorrectPostion;
            tile.transform.position = targetPosition;
            tile.lastCorrectPostion = targetPosition;
        }
        else
        {
            tile.ResetPosition();
        }
    }
}
