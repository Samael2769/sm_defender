using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _mapPrefabs = new List<GameObject>();
    private int _mapSize = 5;
    private GameObject[][] _mapPool;
    private float multiplicateur = 1.95f;
    private List<string> _map = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        _map = CreateMapWithRandomPath(_mapSize, 1);
        Debug.Log(string.Join("\n", _map));
        _mapPool = GenerateMap(_map);
    }

    GameObject[][] GenerateMap(List<string> map)
    {
        GameObject[][] mapPool = new GameObject[_mapSize][];
        for (int i = 0; i < _mapSize; i++)
        {
            mapPool[i] = new GameObject[_mapSize];
            for (int j = 0; j < _mapSize; j++)
            {
                mapPool[i][j] = Instantiate(_mapPrefabs[map[i][j] - '0'], new Vector3(i * multiplicateur, 0, j * multiplicateur), Quaternion.identity);
                mapPool[i][j].transform.parent = transform;
                float rotX = 0;
                if (j + 1 >= _mapSize || map[i][j + 1] != '0' || j - 1 < 0 || map[i][j - 1] != '0')
                {
                    rotX = 90;
                }
                mapPool[i][j].transform.rotation = Quaternion.Euler(90, rotX, 0);
            }
        }
        return mapPool;
    }

List<string> CreateMapWithRandomPath(int mapSize, int pathWidth)
{
    List<string> map = new List<string>();

    // Initialize the map with all "0" values
    for (int i = 0; i < mapSize; ++i)
    {
        map.Add(new string('0', mapSize));
    }

    // Define the path with turns
    System.Random rand = new System.Random();
    int currentX = 0;
    int currentY = rand.Next(mapSize - pathWidth); // Starting Y position within bounds

    while (currentX < mapSize)
    {
        // Set a random path width (can vary path width)
        int pathLength = rand.Next(1, pathWidth + 1);

        // Fill the path for the current segment (horizontal)
        for (int i = 0; i < pathLength; i++)
        {
            if (currentX < mapSize)
            {
                // Place "1" in the path
                map[currentY] = map[currentY].Remove(currentX, 1).Insert(currentX, "1");
                currentX++;
            }
        }

        // Move vertically without diagonals
        int direction = rand.Next(3); // 0 for up, 1 for down, 2 for straight
        if (direction == 0 && currentY > 0)
        {
            // Move up
            for (int i = 0; i < pathWidth; i++)
            {
                if (currentY - i >= 0)
                {
                    map[currentY - i] = map[currentY - i].Remove(currentX - 1, 1).Insert(currentX - 1, "1");
                }
            }
            currentY--;
        }
        else if (direction == 1 && currentY < mapSize - pathWidth)
        {
            // Move down
            for (int i = 0; i < pathWidth; i++)
            {
                if (currentY + i < mapSize)
                {
                    map[currentY + i] = map[currentY + i].Remove(currentX - 1, 1).Insert(currentX - 1, "1");
                }
            }
            currentY++;
        }
    }

    return map;
}





}
