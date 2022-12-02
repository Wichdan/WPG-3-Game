using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] Weapon enemyWeapon;
    [SerializeField] Transform atkPoint;
    [SerializeField] LayerMask targetHit;

    Character charStats;
    Animator weaponAnim;
    Transform playerPos;

    public Animator WeaponAnim { get => weaponAnim; set => weaponAnim = value; }

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        charStats = GetComponent<Character>();

        if (enemyWeapon != null)
        {
            GameObject weapon = Instantiate(enemyWeapon.theWeapon, transform);
            WeaponAnim = weapon.GetComponent<Animator>();
            atkPoint = weapon.transform;

            charStats.Attack += enemyWeapon.bonusAttack;
            charStats.AttackSpeed -= enemyWeapon.attackSpeed;
            weapon.SetActive(true);
        }
    }

    public void Attack()
    {
        if (charStats.IsAttack) return;
        charStats.Attacking(WeaponAnim);
        StartCoroutine(charStats.DisableAttack(charStats.AttackSpeed));
        SpawnHitBox();
    }

    void SpawnHitBox()
    {
        if(charStats.IsStun) return;
        Collider2D col = Physics2D.OverlapCircle(atkPoint.position, enemyWeapon.radius, targetHit);
        if (col != null)
        {
            Character player = col.GetComponent<Character>();
            player.TakeDamage(charStats.Attack);
        }
    }
}
