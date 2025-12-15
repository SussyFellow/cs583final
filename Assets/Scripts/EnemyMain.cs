using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMain : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rb;
    Transform target;
    public GameObject player;
    PlayerMovement playerScript;

    public float startHealth;

    float health;
    public float damagePerAttack;
    public float knockbackForce; //how much force it gets knocked back by when shot
    public float recoveryTime; //how long in seconds it should take to recover after being shot
    float recoveryTimer; //tracks how long it's spending recovering
    public float attackTime;
    float attackTimer; 
    public float rotationSpeed;
    float speed;
    public Animator animator;
    public Collider collider;

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
        animator.SetBool("Walking", true);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target = player.transform;
        playerScript = player.GetComponent<PlayerMovement>();
        speed = agent.speed;
    }

    void Update() //follows whatever object or position is set as its target.
    {
        switch (state)
        {
            case enemyState.Walking: //normal behavior
                agent.SetDestination(target.position);
                health += 33 * Time.deltaTime;
                health = Mathf.Clamp(health, 0, startHealth);
                if (agent.velocity.magnitude < 0.01f) //if not moving, go to idle animation
                {
                    animator.SetBool("Walking", false);
                }
                else
                {
                    animator.SetBool("Walking", true);
                }

                attackTimer -= Time.deltaTime;

                if (agent.remainingDistance < agent.stoppingDistance) //if very close to player, attack and keep facing them
                {
                    agent.updateRotation = false; //control rotation with script
                    Vector3 directionToTarget = target.position - transform.position;
                    directionToTarget.y = 0; //keep rotation horizontal
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //smoothly turn to face player
                    if (attackTimer < 0f) //if enough time passed since last attack
                    {
                        Attack();
                    }
                }
                else
                {
                    agent.updateRotation = true;
                }
                
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
        agent.ResetPath(); //enemy stops going anywhere
        agent.updatePosition = false; //disables automatic movement
        transform.rotation = Quaternion.LookRotation(new Vector3(hit.normal.x, 0f, hit.normal.z));
        rb.isKinematic = false;
        rb.AddForce(hit.normal * knockbackForce * -1, ForceMode.Impulse);
        recoveryTimer = recoveryTime;
        health -= 30;
        if (health <= 0)
        {
            state = enemyState.Dead;
            animator.SetBool("Dead", true);
            rb.isKinematic = true;
            collider.enabled = false;
        }
        else
        {
            state = enemyState.Recoil;
            animator.SetBool("Damaged", true);
        }
    }

    public void Recover()
    {
        rb.isKinematic = true;
        agent.Warp(transform.position);
        agent.updatePosition = true;
        state = enemyState.Walking;
        attackTimer = attackTime;
        animator.SetBool("Damaged", false);
    }

    void Attack()
    {
        attackTimer = attackTime;
        animator.SetBool("Attack", true);
        agent.speed = 0f;
        Invoke(nameof(EndAttack), 0.4f); //about the middle of the attack anim
    }

    void EndAttack()
    {
        animator.SetBool("Attack", false); //make sure it can naturally transition to walking or some other state 
        agent.speed = speed;
        if (agent.remainingDistance < agent.stoppingDistance * 1.2f && state == enemyState.Walking)
        {
            playerScript.TakeDamage(damagePerAttack);
        }
    }
}
