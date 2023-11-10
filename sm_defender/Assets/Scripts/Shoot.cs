using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float fireRange = 5f;
    public float delta_time = 0f;
    public int damage = 10;

    private List<GameObject> _enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _enemies = GameObject.Find("MapGenerator").GetComponent<MapGenerator>()._enemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemies.Count > 0)
        {
            foreach (GameObject enemy in _enemies)
            {
                if (enemy == null)
                    continue;
                if (Vector3.Distance(transform.position, enemy.transform.position) < fireRange)
                {
                    //transform.LookAt(enemy.transform);
                    if (delta_time > fireRate)
                    {
                        Vector3 predictedPosition = predictPosition(enemy.transform.position, enemy.GetComponent<Rigidbody>().velocity, bulletPrefab.GetComponent<Bullet>().speed);
                        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                        transform.LookAt(predictedPosition);
                        bullet.transform.LookAt(predictedPosition);
                        delta_time = 0f;
                        break;
                    }
                }
            }
        }
        delta_time += Time.deltaTime;
    }

    Vector3 predictPosition(Vector3 target, Vector3 targetVelocity, float projectileSpeed)
    {
        float distance = Vector3.Distance(transform.position, target);
        float time = distance / projectileSpeed;
        Vector3 targetFuturePosition = target + targetVelocity * time;
        targetFuturePosition.y = transform.position.y;
        return targetFuturePosition;
    }
}
