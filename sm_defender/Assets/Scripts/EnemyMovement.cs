using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public List<List<int>> _map;
    public List<GameObject> _enemies = new List<GameObject>();
    public int currentX = 0;
    public int currentY = 0;
    private Vector3 _nextPos;
    public float speed = 1f;
    public float tileMult = 0f;
    public bool canMove = true;
    public bool isStart = true;
    public int health = 100;
    public GameObject MapGenerator;
    public float multiplicateur = 1f;
    public float price = 10f;
    public int damage = 1;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //get map from map generator
        _map = GameObject.Find("MapGenerator").GetComponent<MapGenerator>()._mapMatrix;
        tileMult = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().tileMult;
        rb = GetComponent<Rigidbody>();
        _enemies = GameObject.Find("MapGenerator").GetComponent<MapGenerator>()._enemies;
        MapGenerator = GameObject.Find("MapGenerator");
        multiplicateur = MapGenerator.GetComponent<MapGenerator>().multiplicateur;
        speed = speed * multiplicateur;
        health = (int)(health * multiplicateur) + (10 * MapGenerator.GetComponent<MapGenerator>().wave_number);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position != _nextPos && isStart == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextPos, speed * Time.deltaTime);
            transform.LookAt(_nextPos);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            rb.velocity = (_nextPos - transform.position).normalized * speed;
            return;
        }
        if (currentX == _map.Count - 1|| currentY == _map.Count - 1)
        {
            _enemies.Remove(gameObject);
            MapGenerator.GetComponent<MapGenerator>().life -= damage;
            Destroy(gameObject);
            return;
        }
        isStart = false;
        int side1 = _map[currentX + 1][currentY];
        int side2 = _map[currentX][currentY + 1];
        if (side1 == 1 && side2 == 1)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                _nextPos = new Vector3((currentX + 1) * tileMult, 0.6f, currentY * tileMult);
                currentX++;
            }
            else
            {
                _nextPos = new Vector3(currentX * tileMult, 0.6f, (currentY + 1) * tileMult);
                currentY++;
            }
        }
        else if (side1 == 0 && side2 == 1)
        {
            _nextPos = new Vector3(currentX * tileMult, 0.6f, (currentY + 1) * tileMult);
            currentY++;
        }
        else if (side1 == 1 && side2 == 0)
        {
            _nextPos = new Vector3((currentX + 1) * tileMult, 0.6f, currentY * tileMult);
            currentX++;
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _enemies.Remove(gameObject);
            MapGenerator.GetComponent<MapGenerator>().money += (int)(price * multiplicateur);
            Destroy(gameObject);
        }
    }
}
