using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _mapPrefabs = new List<GameObject>();
    [SerializeField] private int _mapSize = 5;

    private List<List<GameObject>> _map = new List<List<GameObject>>();
    public List<List<int>> _mapMatrix = new List<List<int>>();
    public float tileMult = 10f;
    public int nombre_de_chemin = 3;
    private List<Vector3> _enemySpawn = new List<Vector3>();
    public List<GameObject> _enemiesPrefabs = new List<GameObject>();
    public List<GameObject> _enemies = new List<GameObject>();
    public List<GameObject> _towersPrefabs = new List<GameObject>();
    private List<GameObject> _towers = new List<GameObject>();
    public int wave_size = 10;
    public int wave_number = 10;
    public float wave_time = 5f;
    public float enemy_delay = 5f;
    private int _currentEnemy = 0;
    private int _currentWave = 0;
    private float delta_time = 0f;
    private float delta_time_wave = 0f;
    private int wave_spawn = 0;
    public int money = 100;
    public TextMeshProUGUI money_text;
    public float multiplicateur = 1f;
    public TextMeshProUGUI wave_text;
    public TextMeshProUGUI life_text;
    public int life = 10;

    // Start is called before the first frame update
    void Start()
    {
        generateMap();
        multiplicateur = Settings.difficulty;
        wave_spawn = Random.Range(0, _enemySpawn.Count);
        wave_size = (int)(wave_size * multiplicateur);
        wave_time = wave_time / multiplicateur;
        enemy_delay = enemy_delay / multiplicateur;
        wave_number = (int)(wave_number * multiplicateur);
        Debug.Log("difficulty: " + multiplicateur);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentWave < wave_number || Settings.isInfinite == true && delta_time_wave > wave_time)
        {
            if (_currentEnemy < wave_size || Settings.isInfinite == true)
            {
                if (delta_time > enemy_delay)
                {
                    if (_currentWave % 5 == 0 && _currentWave != 0 && _currentEnemy == wave_size - 1)
                        spawnEnemyOnPath(wave_spawn, 2);
                    else if (_currentEnemy == wave_size - 1)
                        spawnEnemyOnPath(wave_spawn, 1);
                    else
                        spawnEnemyOnPath(wave_spawn, 0);
                    _currentEnemy++;
                    delta_time = 0f;
                }
                delta_time += Time.deltaTime;
            }
            else
            {
                _currentWave++;
                _currentEnemy = 0;
                wave_spawn = Random.Range(0, _enemySpawn.Count);
                delta_time_wave = 0f;
            }
        }
        delta_time_wave += Time.deltaTime;
        money_text.text = money.ToString();
        wave_text.text = "Wave: " + (_currentWave + 1).ToString() + "/" + wave_number.ToString() + "\n" + "Enemies: " + (_currentEnemy + 1).ToString() + "/" + wave_size.ToString();
        life_text.text = life.ToString();
        if (life <= 0)
        {
            Debug.Log("Game Over");
            Sm_SceneManager.LoadScene(0);
        }

        if (_currentWave == wave_number && _currentEnemy == wave_size - 1 && _enemies.Count == 0 && Settings.isInfinite == false)
        {
            Debug.Log("You win");
            Sm_SceneManager.LoadScene(0);
        }
    }

    public bool isInPos(float mx, float my, float msize, float x, float y)
    {
        // if x is in mx - size / 2 and mx + size / 2 and y is in my - size / 2 and my + size / 2 then return true
        if (x >= mx - msize / 2 && x <= mx + msize / 2 && y >= my - msize / 2 && y <= my + msize / 2)
            return true;
        return false;
    }

    public bool findTowerPos(float x, float y, int type = 0)
    {
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                if (_mapMatrix[i][j] == 0)
                {
                    if (isInPos(_map[i][j].transform.position.x, _map[i][j].transform.position.z, tileMult, x, y))
                    {
                        Debug.Log("Tower can be placed on " + i + " " + j);
                        createTower(i, j, type);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void createTower(int x, int y, int type = 0)
    {
        Vector3 pos = new Vector3(_map[x][y].transform.position.x, 1f , _map[x][y].transform.position.z);
        GameObject tower = Instantiate(_towersPrefabs[type], pos , Quaternion.identity);
        _towers.Add(tower);
    }

    private void spawnEnemyOnPath(int pindex, int eindex)
    {
        setSpawn();
        GameObject enemy = Instantiate(_enemiesPrefabs[eindex], _enemySpawn[pindex], Quaternion.identity);
        enemy.GetComponent<EnemyMovement>().currentX = (int)_enemySpawn[pindex].x / (int)tileMult;
        enemy.GetComponent<EnemyMovement>().currentY = (int)_enemySpawn[pindex].z / (int)tileMult;
        _enemies.Add(enemy);
    }

    private void spawnEnemies()
    {
        setSpawn();
        for (int i = 0; i < _enemySpawn.Count; i++)
        {
            int random = Random.Range(0, _enemiesPrefabs.Count);
            GameObject enemy = Instantiate(_enemiesPrefabs[random], _enemySpawn[i], Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    private void setSpawn()
    {
        _enemySpawn.Add(new Vector3(0, 0.5f, 0));
        for (int i = 1; i < _mapSize; i++)
        {
            if (_mapMatrix[i][0] == 1)
            {
                _enemySpawn.Add(new Vector3(i * tileMult, 0.5f, 0));
            }
            if (_mapMatrix[0][i] == 1)
            {
                _enemySpawn.Add(new Vector3(0, 0.5f, i * tileMult));
            }
        }
    }

    private void createMatrix()
    {
        for (int i = 0; i < _mapSize; i++)
        {
            _mapMatrix.Add(new List<int>());
            for (int j = 0; j < _mapSize; j++)
            {
                _mapMatrix[i].Add(0);
            }
        }
    }

    private void createPathMatrix()
    {
        int nb_path = Random.Range(1, nombre_de_chemin);
        for (int i = 0; i < nb_path; i++)
        {
            //CurrentX and CurrentY are random numbers on the edge of the map
            int currentX = 0;
            int currentY = 0;

            if (i == 0)
            {
                breakPath(currentX, currentY);
            } else
            {
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    currentX = 0;
                    currentY = Random.Range(0, _mapSize / 2);
                } else
                {
                    currentX = Random.Range(0, _mapSize / 2);
                    currentY = 0;
                }
                breakPath(currentX, currentY);
            }
        }
    }

    private void breakPath(int currentX, int currentY)
    {
        _mapMatrix[currentX][currentY] = 1;
        while ((currentX != _mapSize - 1 && currentY != _mapSize - 1))
        {
            int direction = Random.Range(0, 2);
            if (direction == 0)
            {
                if (currentX != _mapSize - 1)
                {
                    currentX++;
                    _mapMatrix[currentX][currentY] = 1;
                }
            } else
            {
                if (currentY != _mapSize - 1)
                {
                    currentY++;
                    _mapMatrix[currentX][currentY] = 1;
                }
            }
        }
    }

    private void printMatrix()
    {
        for (int i = 0; i < _mapSize; i++)
        {
            string line = "";
            for (int j = 0; j < _mapSize; j++)
            {
                line += _mapMatrix[i][j].ToString() + " ";
            }
            Debug.Log(line);
        }
    }

    private void generateMap()
    {
        createMatrix();
        createPathMatrix();
        for (int i = 0; i < _mapSize; i++)
        {
            _map.Add(new List<GameObject>());
            for (int j = 0; j < _mapSize; j++)
            {
                _map[i].Add(null);
            }
        }
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                if (_mapMatrix[i][j] == 1)
                {
                    GameObject mapPrefab = _mapPrefabs[1];
                    GameObject map = Instantiate(mapPrefab, new Vector3(i * tileMult, 0, j * tileMult), Quaternion.identity);
                    map.transform.Rotate(90, 0, 0);
                    _map[i][j] = map;
                } else
                {
                    GameObject mapPrefab = _mapPrefabs[0];
                    GameObject map = Instantiate(mapPrefab, new Vector3(i * tileMult, 0, j * tileMult), Quaternion.identity);
                    map.transform.Rotate(90, 0, 0);
                    _map[i][j] = map;
                }
            }
        }
    }

    public bool addMoney(int amount)
    {
        money += amount;
        if (money < 0)
        {
            money -= amount;
            return false;
        }
        return true;
    }

}
