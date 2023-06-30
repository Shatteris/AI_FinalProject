using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Spotter : MonoBehaviour
{


    [SerializeField] float EnemySpd = 2.0f;
    [SerializeField] float FieldView = 4.0f;
    [SerializeField] float FieldDistance = 4.0f;

    private Rigidbody rb;
    public Transform PlayerPos;
    private Vector3 LastPlayerPos;

    private Vector3 Post;
    private bool isTurning = false;
    private float turnDuration = 2f;
    private float turnTimer = 5f;

    private bool PlayerSpotted = false;
    private bool Aware = false;

    public float spottingRange = 5f;
    public float warningRange = 20f;
    public float warningDuration = 5f;

    private void Awake()
    {
        Post = transform.position;
    }
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        

        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        //Aim FoV forward...........................................................
        Vector3 Forward = transform.forward;
        Vector3 viewDirection = new Vector3(Forward.x, 0f, Forward.z).normalized;
        transform.rotation = Quaternion.LookRotation(viewDirection);
        //..........................................................................


        //Turn...........................
        if (isTurning)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0f)
            {
                isTurning = false;
                turnTimer = 0f;
            }
            //..............................

        }
    }


    //Detect Player and Warn others.........................................................
    [Task]
    public bool DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (PlayerSpotted == false)
        {

            if (player != null)
            {
                if (distance <= spottingRange)
                {
                    SpotPlayer(player.transform.position);
                }
            }
        }
        if ((distance >= spottingRange) && (PlayerSpotted == true))
        {
            StartCoroutine(DisableWarning());

        } return true;

    }
    //......................................................................................
    [Task]
    public bool Turn()
    {
        if (!isTurning)
        {
            isTurning = true;
            turnTimer = turnDuration;

            Quaternion ViewRotation = Quaternion.Euler(0f, transform.eulerAngles.y + 90f, 0f);
            StartCoroutine(TurnCoroutine(ViewRotation));
        }
        return true;
    }
                private IEnumerator TurnCoroutine(Quaternion VRotation)
                {
                    Quaternion startRotation = transform.rotation;
                    float t = 0f;

                    while(t < 1f)
                    {
                        t += Time.deltaTime / turnDuration;
                        transform.rotation = Quaternion.Lerp(startRotation, VRotation, t);
                        yield return null;
                    }
                }
        

    [Task]
    public bool Origin()
    {
        transform.position = Post;
        return true;
    }

    [Task]
    public bool ChasePlayer()
    {
        Vector3 direction = (PlayerPos.position - transform.position).normalized;

        Vector3 Chase_Path = direction * EnemySpd * Time.deltaTime;

        transform.position += Chase_Path;
        return true;
    }

    [Task]
    public bool SeePlayer()
    {
        Vector3 directionToPlayer = PlayerPos.position - transform.position;

        if (Vector3.Angle(transform.forward, directionToPlayer) < FieldView * 0.5f)
        {
            if (directionToPlayer.magnitude < FieldDistance)
            {
                Aware = true;
                LastPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

            }
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    public bool BlindChase()
    {
        Vector3 LastDirection = (LastPlayerPos - transform.position).normalized;
        Vector3 LastChase_Path = LastDirection * EnemySpd * Time.deltaTime;

        transform.position += LastChase_Path;
        return true;
    }

    [Task]
    public bool AwareOfPlayer()
    {
        return Aware;
    }

    [Task]
    public bool ForgetPlayer()
    {
        Aware = false;
        return true;
    }

    private void SpotPlayer(Vector3 position)
    {

        PlayerSpotted = true;

       Chaser[] chasers = FindObjectsOfType<Chaser>();
        foreach (Chaser chaser in chasers)
        {
            if (chaser != this)
            {
                float distance = Vector3.Distance(transform.position, chaser.transform.position);
                if (distance <= warningRange)
                {
                    chaser.ChasePlayer();
                }
            }
        }
    }

    private IEnumerator DisableWarning()
    {
        yield return new WaitForSeconds(warningDuration);
        PlayerSpotted = false;
    }

}
