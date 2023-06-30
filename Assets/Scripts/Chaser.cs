using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class Chaser : MonoBehaviour
{

    [SerializeField] float FieldView = 4.0f;
    [SerializeField] float FieldDistance = 4.0f;

    [SerializeField] float EnemySpd = 3.0f;
    
    private Transform EnemyPos;
    public Transform PlayerPos;
    private Vector3 LastPlayerPos;

    private Vector3 PatrolTarget;
    private int CurrentPatrolPos;
    public Transform[] PatrolPoints;

    private bool Aware = false;

    private Rigidbody rb;
    [SerializeField] NavMeshAgent agent;


    void Start()
    {
        

        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();

    }


    void Update()
    {
        EnemyPos = this.transform;
    }

    [Task]
    public bool Patrol()
    {
        if (PatrolPoints.Length > 0)
        {
            PatrolTarget = PatrolPoints[CurrentPatrolPos].position;

            transform.position = Vector3.MoveTowards(transform.position, PatrolTarget, agent.speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, PatrolTarget) < 0.1f)
            {
                CurrentPatrolPos++;

                if (CurrentPatrolPos >= PatrolPoints.Length)
                {
                    CurrentPatrolPos = 0;
                }
            }
        }
        return true;
    }


    [Task]
    public bool DetectPlayer()
    {
        Vector3 directionToPlayer = PlayerPos.position - transform.position;

        if(Vector3.Angle(transform.forward, directionToPlayer)< FieldView * 0.5f)
        {
            if (directionToPlayer.magnitude < FieldDistance)
            {
                Aware = true;
            }
            return true;
        }

     return false;

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
    public bool ChasePlayer()
    {
            Vector3 direction = (PlayerPos.position - EnemyPos.position).normalized;

            Vector3 Chase_Path = direction * EnemySpd * Time.deltaTime;

            transform.position += Chase_Path;
            return true;
    }

    [Task]
    public bool BlindChase()
    {
        Vector3 LastDirection = (LastPlayerPos - EnemyPos.position).normalized;
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
}
