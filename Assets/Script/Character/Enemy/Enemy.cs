using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Slider hpSlider;

    [Header("Enemy Settings")]
    [SerializeField] Vector2 movement;
    [SerializeField] float patrolCD, chaseDistance;
    float tempPatrolCD;
    [SerializeField] bool isPatrol, isMove, isChase;
    Rigidbody2D rb;
    Transform playerPos;
    Character charStats;
    EnemyCombat enemyCombat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        charStats = GetComponent<Character>();
        hpSlider = GetComponentInChildren<Slider>();
        enemyCombat = GetComponent<EnemyCombat>();

        tempPatrolCD = patrolCD;
        hpSlider.maxValue = charStats.HealthPoint;
    }

    private void Update()
    {
        hpSlider.value = charStats.HealthPoint;

        if(charStats.HealthPoint <= 0)
            charStats.Defeated();

        if (isChase)
            isPatrol = false;
        else
            isPatrol = true;

        Chasing();

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
            rb.MovePosition(rb.position + movement * charStats.MoveSpeed * Time.fixedDeltaTime);
        else if (isChase)
            rb.position = Vector2.MoveTowards(transform.position, playerPos.position, charStats.MoveSpeed * Time.fixedDeltaTime);
        else if (!isMove && !isChase)
        {
            rb.velocity = new Vector2(0, 0);
            charStats.charAnim.SetFloat("Speed", 0);
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
            charStats.charAnim.SetFloat("moveX", movement.x);
            charStats.charAnim.SetFloat("moveY", movement.y);

            if (enemyCombat.WeaponAnim != null)
            {
                enemyCombat.WeaponAnim.SetFloat("moveX", movement.x);
                enemyCombat.WeaponAnim.SetFloat("moveY", movement.y);
            }
        }

        charStats.charAnim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void Chasing()
    {
        if(charStats.IsStun) return;
        chaseDistance = Vector2.Distance(transform.position, playerPos.position);
        if (chaseDistance < 2)
        {
            isChase = true;
            FacingPlayer();
        }
        else
            isChase = false;

        if (isChase && chaseDistance <= 1.2f)
        {
            enemyCombat.Attack();
        }
    }

    void FacingPlayer()
    {
        Vector2 facing = (transform.position - playerPos.position).normalized;
        
        charStats.charAnim.SetFloat("moveX", -facing.x);
        charStats.charAnim.SetFloat("moveY", -facing.y);

        if (enemyCombat.WeaponAnim != null)
        {
            enemyCombat.WeaponAnim.SetFloat("moveX", -facing.x);
            enemyCombat.WeaponAnim.SetFloat("moveY", -facing.y);
        }

        charStats.charAnim.SetFloat("Speed", facing.sqrMagnitude);
    }

    void FlipMove() => isMove = !isMove;
}
