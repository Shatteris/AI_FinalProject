using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERA : MonoBehaviour
{
    public Transform Player_Transform;

    private Vector3 camera_OffSet;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    public bool LookAtPlayer = false;

    private void Start()
    {
        camera_OffSet = transform.position - Player_Transform.position;
    }

    //For camera smooth effect..............................................................
    private void LateUpdate()
    {
        Vector3 newPos = Player_Transform.position + camera_OffSet;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    //.....................................................................................

    //Camera Focus on Player...........................
        if(LookAtPlayer)
        {
            transform.LookAt(Player_Transform);
        }
    //.................................................
    }
}
