using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    static public float difficulty = 1f;
    static public bool isInfinite = false;

    public void setDifficulty(float diff)
    {
        difficulty = 1 + diff;
    }

    public void setInfinite(bool inf)
    {
        isInfinite = inf;
    }
}
