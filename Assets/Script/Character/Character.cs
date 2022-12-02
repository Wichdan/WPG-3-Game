using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int healthPoint, defense;
    [SerializeField] private int attack;
    [SerializeField] private float attackSpeed, stamina, mana, maxStamina, maxMana, invincibleCD;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Condition")]
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isMove, isDodge, isInvincible;

    [Header("Effect")]
    [SerializeField] private bool isStun;
    [SerializeField] private bool isBurn, isBlind;

    [Header("Reference")]
    public Animator charAnim;

    float tempMoveSpeed;

    public int HealthPoint { get => healthPoint; set => healthPoint = value; }
    public int Attack { get => attack; set => attack = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float Mana { get => mana; set => mana = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsDodge { get => isDodge; set => isDodge = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float MaxMana { get => maxMana; set => maxMana = value; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public int Defense { get => defense; set => defense = value; }
    public bool IsStun { get => isStun; set => isStun = value; }
    public bool IsBurn { get => isBurn; set => isBurn = value; }
    public bool IsBlind { get => isBlind; set => isBlind = value; }

    private void Awake()
    {
        charAnim = GetComponent<Animator>();
        tempMoveSpeed = moveSpeed;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;
        charAnim.SetTrigger("Hurt");
        if (defense > 0)
        {
            defense -= 1;
        }
        else
            this.HealthPoint -= dmg;
        Invincible();
    }

    public void Heal(int heal)
    {
        this.HealthPoint += heal;
    }

    public void DecreaseStamina(float stamina)
    {
        this.stamina -= stamina;
    }

    public void Attacking(Animator weaponAnim)
    {
        isAttack = true;
        charAnim.SetTrigger("Attack");
        weaponAnim.SetTrigger("Attack");
    }

    public IEnumerator DisableAttack(float countDown)
    {
        yield return new WaitForSeconds(countDown);
        isAttack = false;
    }

    public void KnockBack(Rigidbody2D rb, Vector2 knockback, float knockForce)
    {
        rb.MovePosition(rb.position + knockback * knockForce * Time.fixedDeltaTime);
    }

    public void Invincible()
    {
        isInvincible = true;
        StartCoroutine(DisableInvincible());
    }

    IEnumerator DisableInvincible()
    {
        yield return new WaitForSeconds(invincibleCD);
        isInvincible = false;
    }

    public IEnumerator WhenInvincible(SpriteRenderer sp, Color color)
    {
        yield return new WaitForSeconds(1f);
        charAnim.enabled = false;
        sp.color = color;
        yield return new WaitForSeconds(0.1f);
        charAnim.enabled = true;
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public IEnumerator Stun()
    {
        moveSpeed = 0;
        IsStun = true;
        yield return new WaitForSeconds(5f);
        moveSpeed = tempMoveSpeed;
        IsStun = false;
    }

    void Burn()
    {

    }

    void Blind()
    {

    }
}
