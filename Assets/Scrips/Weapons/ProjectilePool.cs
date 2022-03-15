using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Realtime;
using UnityEngine;

[System.Serializable]public class ProjectilePool
{
    [SerializeField] GameObject prefab;
    [SerializeField] int size;

    public int Size => size;

    public int RuntimeSize => queue.Count;
    
    Queue<GameObject> queue;

    private Transform parent;
    public void Initialize(Transform _parent)
    {
        queue = new Queue<GameObject>();
        parent = _parent;
        
        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy(parent));
        }
    }

    GameObject Copy(Transform parent)
    {
        var copy = GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }

    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy(parent);
            Debug.Log("对象池越界");
        }

        queue.Enqueue(availableObject);
        
        return availableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();
        
        preparedObject.SetActive(true);

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        
        preparedObject.SetActive(true);
        Debug.Log(preparedObject.transform.position.x);

        preparedObject.transform.position = position;

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        
        preparedObject.SetActive(true);

        preparedObject.transform.position = position;

        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }
    
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Player owner ,float _initialSpeed)
    {
        GameObject preparedObject = AvailableObject();
        
        preparedObject.GetComponent<Projectile>().owner = owner;
        
        preparedObject.GetComponent<Projectile>().initialSpeed = _initialSpeed;
        
        preparedObject.SetActive(true);

        preparedObject.transform.position = position;

        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }
}
