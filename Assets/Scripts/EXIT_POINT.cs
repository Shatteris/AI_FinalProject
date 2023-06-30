using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXIT_POINT : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MANAGER.Instance.GameEscape();
        }
    }
}
