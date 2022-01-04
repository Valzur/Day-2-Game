using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Prefabs
{
    public Tile groundTilePrefab;
    public Tile pathTile;
    public EnemySpawner entrancePrefab;
    public Tile exitPrefab;
}
public class GameManager : Shop
{
    public static GameManager Instance;
    public UIManager uIManager;
    [SerializeField] Camera camera;
    [SerializeField] Prefabs prefabs;
    [SerializeField] Transform tilesHolder;
    [SerializeField] float minPathLengthFactor = 1;
    EnemySpawner spawner;
    int lives = 3;
    Tile[,] groundTiles;
    int _hexes = 200;
    int hexes
    {
        get{return _hexes;}
        set{_hexes = value; uIManager.gamePanel.UpdateHexes(_hexes);}
    }
    Vector2Int[] path;
    Vector2Int size;
    readonly Vector2Int[] legalMoves =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    #region Testing
    [SerializeField] Vector2Int Size;
    [SerializeField] bool generateTiles;
    [SerializeField] bool buyTurret;
    void OnValidate()
    {
        if(!UnityEditor.EditorApplication.isPlaying)
            return;

        if (generateTiles)
        {
            generateTiles = false;
            GenerateRandomGame(Size);
        }
        
        if(buyTurret)
        {
            buyTurret = false;
            ActivateBuyMode(0);
        }
    }
    #endregion

    void Awake() => Instance = this;

    public void GenerateRandomGame(Vector2Int size)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        // Randomize entrance
        Vector2Int entrance = Vector2Int.zero;
        entrance.x = Random.Range(0, size.x);
        if (entrance.x == 0 || entrance.x == size.x)
        {
            // It's at the left or right sides.
            entrance.y = Random.Range(0, size.y);
        }
        else
        {
            // It's in the middle.
            bool isTop = Utility.RandomBool();
            entrance.y = isTop ? size.y - 1 : 0;
        }

        path.Add(entrance);
        // Randomize Path
        bool isEndReached = false;
        int flag = 0;
        int bigFlag = 0;
        bool bugExists = true;
        while(bugExists)
        {
            bugExists = false;
            path.RemoveRange(0, path.Count);
            path.Add(entrance);
            flag = 0;
            bigFlag ++;
            isEndReached = false;
            if(bigFlag > 100)
                break;
            
            while(!isEndReached)
            {
                flag ++;
                if(flag > size.x * size.y)
                {
                    print("Taking more than it's supposed to.. , path so far: " + path);
                    break;
                }
                
                List<Vector2Int> moves = GetAvailableChoices(path[path.Count -1], size, path.ToArray(), CanEnd(path.Count, size) );
                int choice = Random.Range(0, moves.Count);
                if(moves.Count == 0)
                {
                    bugExists = true;
                    break;
                }
                path.Add(path[path.Count -1] + moves[choice]);
                if(IsSide(path[path.Count -1], size))
                    isEndReached = true;
            }
        }
        if(bigFlag < 100)
            GenerateGame(size, path.ToArray());
    }

    public void GenerateGame(Vector2Int size, Vector2Int[] path)
    {
        GenerateTiles(size);
        // Generate Path
        foreach (var tile in path)
        {
            Destroy(groundTiles[tile.x, tile.y].gameObject);
            if(tile == path[0])
            {
                spawner = Instantiate(prefabs.entrancePrefab, Utility.GetVector3(tile), Quaternion.identity, tilesHolder);
                spawner.Setup(path);
                groundTiles[tile.x, tile.y] = spawner;
            }
            else if(tile == path[path.Length-1])
            {
                groundTiles[tile.x, tile.y] = Instantiate(prefabs.exitPrefab, Utility.GetVector3(tile), Quaternion.identity, tilesHolder);
            }
            else
            {
                groundTiles[tile.x, tile.y] = Instantiate(prefabs.pathTile, Utility.GetVector3(tile), Quaternion.identity, tilesHolder);
            }
        }
        // Setup Cam
        camera.transform.position = new Vector3( (size.x -1)/(float)2.0, (size.y-1)/(float)2.0, -2);
        camera.orthographicSize = (float)(0.7) * size.y;
    }

    void GenerateTiles(Vector2Int size)
    {
        // Clear previous
        if (groundTiles != null)
        {
            foreach (var item in groundTiles)
            {
                if(item!= null)
                    Destroy(item.gameObject);
            }
        }

        groundTiles = new Tile[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                groundTiles[x, y] = Instantiate(prefabs.groundTilePrefab, new Vector3(x, y, 0), Quaternion.identity, tilesHolder);
            }
        }
    }

    List<Vector2Int> GetAvailableChoices(Vector2Int oldPos, Vector2Int size, Vector2Int[] path, bool canEnd)
    {
        List<Vector2Int> Choices = new List<Vector2Int>();

        foreach (var move in legalMoves)
        {
            bool isClean = true;
            Vector2Int newPos = oldPos + move;
            if (newPos.x >= 0 && newPos.x < size.x && newPos.y >= 0 && newPos.y < size.y)
            {
                foreach (var tile in path)
                {
                    if (newPos == tile)
                    {
                        //print("Broke as pos: " + newTilePos + " is equal to tile: " + tile);
                        isClean = false;
                        break;
                    }
                }
            }
            else 
            {
                //print("Broke as it is out of boundaries: " + newTilePos + " of size: " + size);
                isClean = false;
            }

            if(IsSide(newPos, size) && !canEnd)
            {
                isClean = false;
            }

            if (isClean) 
            {
                Choices.Add(move);
                // Increase the chances of getting it
                if(IsAwayFromEntrance(move, path[0], size))
                    Choices.Add(move);
            }
        }
        return Choices;
    }

    bool IsSide(Vector2Int pos, Vector2Int size)
    {
        return (pos.x == 0 || pos.x == size.x -1 || pos.y == 0 || pos.y == size.y -1);
    }
    
    bool IsAwayFromEntrance(Vector2Int move, Vector2Int entrance, Vector2Int size)
    {
        bool away = false;
        if(
            (entrance.x == 0         && move == Vector2Int.right) ||
            (entrance.x == size.x -1 && move == Vector2Int.left)  ||
            (entrance.y == 0         && move == Vector2Int.up)    ||
            (entrance.y == size.y -1 && move == Vector2Int.down)
        )   away = true;

        return away;
    }
    
    bool CanEnd(int currentLength, Vector2Int size)
    {
        int maxLength = Mathf.FloorToInt(Mathf.Min(size.x, size.y) * minPathLengthFactor);
        return currentLength >= maxLength;
    }
    
    public void AddHexes(int hexes) => this.hexes += hexes;

    public void LoseHexes(int hexes) => this.hexes -= hexes;
    
    public void LoseHP(Enemy enemy)
    {
        lives --;
        uIManager.gamePanel.UpdateLives(lives);
        if(lives <= 0)
        {
            uIManager.LoseScreen(Mathf.FloorToInt(spawner.totalTime), hexes);
            spawner.StopSimulating();
        }
    }

    public void Quit() => Application.Quit();

    public void ClearGrid()
    {
        // Clear the grid.
        if (groundTiles != null)
        {
            foreach (var item in groundTiles)
                Destroy(item.gameObject);
        }
        groundTiles = null;
    }
}