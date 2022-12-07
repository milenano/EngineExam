using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DuckPool
{
    private static Dictionary<string, Pools> pools = new Dictionary<string, Pools>();

    public static void Reset()
    {
        pools.Clear();
        Debug.Log(pools.Count);
    }

    public static void Active(GameObject duck, Vector3 pos, Quaternion rot)
    {
        GameObject obj;
        string key = duck.name.Replace("(Clone)", "");

        if (pools.ContainsKey(key))
        {
            if (pools[key].inactive.Count == 0)
            {
                Object.Instantiate(duck, pos, rot, pools[key].Enemy.transform);
            }
            else
            {
                obj = pools[key].inactive.Pop();
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(true);
            }
        }
        else
        {
            GameObject newEnemy = new GameObject($"{key}_POOL");
            Object.Instantiate(duck, pos, rot, newEnemy.transform);
            Pools newPools = new Pools(newEnemy);
            pools.Add(key, newPools);
        }
    }

    public static void Unactive(GameObject duck)
    {
        string key = duck.name.Replace("(Clone)", "");

        if (pools.ContainsKey(key))
        {
            pools[key].inactive.Push(duck);
            duck.transform.position = pools[key].Enemy.transform.position;
            duck.SetActive(false);
        }
        else
        {
            GameObject newDuck = new GameObject($"{key}_POOL");
            Pools newPool = new Pools(newDuck);

            duck.transform.SetParent(newDuck.transform);

            pools.Add(key, newPool);
            pools[key].inactive.Push(duck);
            duck.SetActive(false);
        }
    }
}
