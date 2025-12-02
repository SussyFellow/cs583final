using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rb;
    public Transform target;

    public float startHealth;

    float health;
    public float knockbackForce; //force with which to knock back
    public float recoveryTime;
    float recoveryTimer;

    

    public enum enemyState
    {
        Walking,
        Recoil,
        Dead,
        Other
    }

    enemyState state;

    void Awake()
    {
        health = startHealth;
        state = enemyState.Walking;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() //follows whatever object or position is set as its target.
    {
        switch (state)
        {
            case enemyState.Walking:
                agent.SetDestination(target.position);
                health += 33 * Time.deltaTime;
                health = Mathf.Clamp(health, 0, startHealth);
                break;
            case enemyState.Recoil:
                recoveryTimer -= Time.deltaTime;
                if (recoveryTimer < 0)
                {
                    Recover();
                }
                break;
        }
    }


    public void GetShot(RaycastHit hit)
    {
        Debug.Log("I was hit!");
        agent.ResetPath(); //enemy stops going anywhere
        agent.updatePosition = false; //disables automatic movement
        rb.isKinematic = false;
        rb.AddForce(hit.normal * knockbackForce * -1, ForceMode.Impulse);
        recoveryTimer = recoveryTime;
        health -= 30;
        if (health <= 0)
        {
            state = enemyState.Dead;
        }
        else
        {
            state = enemyState.Recoil;
        }
    }

    public void Recover()
    {
        rb.isKinematic = true;
        agent.Warp(transform.position);
        agent.updatePosition = true;
        state = enemyState.Walking;
    }
}
