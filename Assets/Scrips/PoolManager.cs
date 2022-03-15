using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    [SerializeField] ProjectilePool[] projectilePools;

    static Dictionary<GameObject, ProjectilePool> dictionary;
    private void Start()
    {
        dictionary = new Dictionary<GameObject, ProjectilePool>();
        Initialize(projectilePools);
    }
    
    void Initialize(ProjectilePool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.GetPrefab()))
            {
                Debug.Log("相同了");
                continue;
            }
#endif
            dictionary.Add(pool.GetPrefab(), pool);
            Transform poolParent = new GameObject( pool.GetPrefab().name+"----pool" ).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    

    /// <summary>
    /// 这是池的释放函数捏
    /// </summary>
    /// <param name="prefab">释放的预制体</param>
    /// <returns>释放对象</returns>
    public static GameObject Release(GameObject prefab)
    {
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("找不到prefab");
            return null;
        }
        return dictionary[prefab].PreparedObject();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("找不到prefab");
            return null;
        }
        return dictionary[prefab].PreparedObject(position);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("找不到prefab");
            return null;
        }
        return dictionary[prefab].PreparedObject(position,rotation);
    }
    
    /// <summary>
    /// 子弹类的池释放函数
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation,Player owner,float initialSpeed)
    {
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.Log("找不到prefab");
            return null;
        }
        return dictionary[prefab].PreparedObject(position,rotation,owner,initialSpeed);
    }

    /// <summary>
    /// 检测是否池越界
    /// </summary>
    /// <param name="pools"></param>
    void CheckPoolSize(ProjectilePool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("pool:{0} has a runtime size {1} bigger than {2}",
                    pool.GetPrefab().name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }
    
    
    
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(projectilePools);
    }
#endif
}
