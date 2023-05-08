using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (Instance == null)
        {
            Instance = this as T;
            return;
        }

        Destroy(gameObject);
    }
}
