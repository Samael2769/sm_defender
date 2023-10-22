using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 5f;
    public List<GameObject> _sensors = new List<GameObject>();
    private Vector3 _nextPosition;
    private bool _canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //sensors are children of the enemy
        foreach (Transform child in transform.GetChild(1).gameObject.transform)
        {
            _sensors.Add(child.gameObject);
        }
        Debug.Log(_sensors[0].GetComponent<sensor>().isRoad);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _sensors.Count; i++)
        {
            if (_sensors[i].GetComponent<sensor>().isRoad == true && _canMove == true && _sensors[i].GetComponent<sensor>().marked == false)
            {
                _nextPosition = _sensors[i].transform.position;
                _canMove = false;
                for (int j = 0; j < _sensors.Count; j++)
                {
                    _sensors[j].GetComponent<sensor>().marked = false;
                }
                //Changer ça, faire en sorte qu'il marque la position actuelle plutôt que la prochaine position
                //Eviter qu'il revienne sur ses pas
                _sensors[i].GetComponent<sensor>().marked = true;
                break;
            }
        }
        Debug.Log(_nextPosition + " " + transform.position + " " + _canMove);
        if (_nextPosition != null) {
            rb.MovePosition(Vector3.MoveTowards(transform.position, _nextPosition, speed * Time.deltaTime));
            if (transform.position.x >= _nextPosition.x - 0.01 && transform.position.x <= _nextPosition.x + 0.01 && transform.position.z >= _nextPosition.z - 0.01 && transform.position.z <= _nextPosition.z + 0.01)
            {
                Debug.Log("can move");
                _canMove = true;
            }
        }
    }
}
