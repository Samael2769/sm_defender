using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeToLive = 5f;
    public int damage = 10;
    public GameObject turret;
    // Start is called before the first frame update
    void Start()
    {
        damage = turret.GetComponent<Shoot>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<EnemyMovement>().takeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<EnemyMovement>().takeDamage(damage);
    }
}
