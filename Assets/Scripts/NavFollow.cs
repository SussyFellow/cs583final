using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavFollow : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    void Update() //follows whatever object or position is set as its target.
    {
        agent.SetDestination(target.position);
    }
}
