using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] Vector2 movement;
    [SerializeField] float moveSpeed;
    [SerializeField] float patrolCD;
    float tempPatrolCD;
    [SerializeField] bool isPatrol, isMove, isFacingPlayer;
    Animator charAnim;
    Rigidbody2D rb;
    Transform playerPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        tempPatrolCD = patrolCD;
    }

    private void Update()
    {
        FacingPlayer();

        if (!isPatrol) return;
        patrolCD -= Time.deltaTime;
        if (patrolCD <= 0)
        {
            patrolCD = tempPatrolCD;
            FlipMove();
            if (isMove)
                PatrolMove();
        }
    }

    private void FixedUpdate()
    {
        if (isPatrol && isMove)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        else
        {
            rb.velocity = new Vector2(0, 0);
            charAnim.SetFloat("Speed", 0);
        }
    }

    void PatrolMove()
    {
        if (!isMove) return;
        movement.x = Random.Range(-1, 2);
        movement.y = Random.Range(-1, 2);

        movement.Normalize();
        if (movement != Vector2.zero)
        {
            charAnim.SetFloat("moveX", movement.x);
            charAnim.SetFloat("moveY", movement.y);
        }

        charAnim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FacingPlayer()
    {
        if(!isFacingPlayer) return;
        float distance = Vector2.Distance(transform.position, playerPos.position);

        if (distance < 2)
        {
            Vector2 facing = (transform.position - playerPos.position).normalized;
            charAnim.SetFloat("moveX", -facing.x);
            charAnim.SetFloat("moveY", -facing.y);
        }
    }

    void FlipMove() => isMove = !isMove;
}
