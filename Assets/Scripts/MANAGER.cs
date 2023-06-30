using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MANAGER : MonoBehaviour
{
    public static MANAGER Instance { get; private set;}

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GameEscape()
    {
        Debug.Log("Escaoed Sucessfully! Phew...");
    }
}
