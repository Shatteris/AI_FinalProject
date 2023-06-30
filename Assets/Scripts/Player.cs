using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] float player_speed = 5.0f;

    private Rigidbody rb;

    private Vector3 player_Spawn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        player_Spawn = transform.position;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 player_move = new Vector3(horizontalInput, 0f, verticalInput) * player_speed;
        rb.velocity = player_move;

        if (player_move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player_move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    //Respawn................................................
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Spotter"))
        {
            transform.position = player_Spawn;
        }
    }
    //.......................................................
}
