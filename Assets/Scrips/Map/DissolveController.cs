using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private TilemapDissolve[] tilemapDissolves;
    private void Awake()
    {
        tilemapDissolves = GetComponentsInChildren<TilemapDissolve>();
    }

    public void show()
    {
        Debug.Log("showmap");
        foreach (var dissolves in tilemapDissolves)
        {
            Debug.Log("dooooooooooooooooo");
            dissolves.show();
        }
    }
}
