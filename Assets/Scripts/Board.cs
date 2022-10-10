using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public readonly int boardSize = 3; // represents the number of rows and columns
    private readonly int shuffleMoves = 20; // number of moves made to shuffle the 
    private bool isShuffling;
    private Vector2Int previousMove = Vector2Int.zero; // saves the last move used during a shuffle loop
    private Tile emptySpace; // the tile that acts as an empty space to move other tiles
    private Tile[,] tiles; // array of tiles that form the board

    public event Action<Board> OnGameWon; // called when puzzle is solved

    [SerializeField]
    private Texture puzzleImage; // the image of the puzzle to solve
    [SerializeField]
    private Shader shader;


    public void CreateBoard()
    {
        // create an array of size 3*3
        tiles = new Tile[boardSize, boardSize];

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // first tile position is in bottom left, full board is centered on 0
                Vector2 position = new Vector2((boardSize - 1) * -1 * 0.5f, (boardSize - 1) * -1 * 0.5f);

                position += new Vector2(x, y);

                // tile creation
                Tile tile = GameObject.CreatePrimitive(PrimitiveType.Quad).AddComponent<Tile>();
                tile.Initialize(position, new Vector2Int(x, y), shader, puzzleImage, boardSize);

                // make the board a parent for clarity in the hierarchy
                tile.transform.parent = transform;

                // listen to tile movement
                tile.OnTileMoved += MoveTile;

                // removing the middle piece and saving it
                if (x == Mathf.RoundToInt(boardSize / 2) && y == Mathf.RoundToInt(boardSize / 2))
                {
                    emptySpace = tile;

                    // turn tile invisible
                    tile.GetComponent<MeshRenderer>().enabled = false;

                    // changing layer to check for legal moves when moving tiles
                    tile.gameObject.layer = LayerMask.NameToLayer("Empty Space");
                }

                // adding this tile to the board array
                tiles[x, y] = tile;
            }
        }
        Shuffle();
    }

    // make random moves to shuffle the board
    private void Shuffle()
    {
        isShuffling = true;
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

            // save the move to avoid back and forth
            previousMove = move;
        }
        isShuffling = false;
    }

    // switch places between tile and empty space 
    void MoveTile(Tile tile)
    {
        //check if move is legal
        if ((tile.coordinates - emptySpace.coordinates).sqrMagnitude == 1)
        {
            // switching coordinates
            Vector2Int targetCoord = emptySpace.coordinates;
            emptySpace.coordinates = tile.coordinates;
            tile.coordinates = targetCoord;

            // switching tiles on the board
            tiles[emptySpace.coordinates.x, emptySpace.coordinates.y] = emptySpace;
            tiles[targetCoord.x, targetCoord.y] = tile;

            // switching position in world
            Vector3 targetPosition = emptySpace.transform.position;
            emptySpace.transform.position = tile.lastCorrectPosition;
            emptySpace.lastCorrectPosition = tile.lastCorrectPosition;
            tile.transform.position = targetPosition;
            tile.lastCorrectPosition = targetPosition;
        }
        else
        {
            // places the tile back to where it was before being picked up
            tile.ResetPosition();
        }

        // check if game is solved after every move if the shuffling is done
        if (!isShuffling && IsSolved())
        {
            OnGameWon?.Invoke(this);
        }
    }

    private bool IsSolved()
    {
        // go through the board to find any tile in the wrong place
        foreach (Tile tile in tiles)
        {
            if (tile.coordinates != tile.correctCoordinates)
            {
                return false;
            }
        }

        // if none are found, puzzle is solved;
        return true;
    }
}
