using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    //public Animator anim;
    private bool IsClosed = true;
    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
            {
                if(IsClosed == true)
                {
                    transform.localPosition += new Vector3(0, 3, 0);
                    IsClosed = false;
                }

                 //if (IsClosed == false)
                 //{
                  //  transform.localPosition -= new Vector3(0, 3, 0);
                 //   IsClosed = true;
                 //}
            }

        }
    }

}
