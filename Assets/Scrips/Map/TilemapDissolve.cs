using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDissolve : MonoBehaviour
{
    private Material dissolve;
    private bool isDissolving = false;
    [SerializeField]private float fade = 0;
    
    private void Start()
    {
        dissolve = GetComponent<TilemapRenderer>().material;
    }

    private void Update()
    {
        if(isDissolving) showMap();
    }

    public void showMap()
    {
        fade += Time.deltaTime/2;
        if (fade >= 1f)
        {
            fade = 1f;
            isDissolving = false;
        }
        dissolve.SetFloat("_Fade",fade);
    }

    public void show()
    {
        isDissolving = true;
        dissolve.SetFloat("_Fade",0);
        Debug.Log("true");
    }
    
}
