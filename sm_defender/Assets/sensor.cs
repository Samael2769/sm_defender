using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensor : MonoBehaviour
{

    public bool isRoad = false;
    public bool marked = false;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Road")
        {
            isRoad = true;
        } else
        {
            isRoad = false;
        }
    }
}
