using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] Weapon playerWeapon;
    [SerializeField] int weaponIndex;
    [SerializeField] Weapon[] weaponSlot;
    [SerializeField] GameObject[] weapons;
    [SerializeField] Transform atkPoint;
    [SerializeField] LayerMask targetHit;
    [SerializeField] Image weaponIcon;

    [Header("Reference")]
    [SerializeField] LevelManager levelManager;
    [SerializeField] Animator weaponAnim;
    Player player;
    Character charStats;
    float tempAtkSpeed;
    int tempAtk;

    public Animator WeaponAnim { get => weaponAnim; set => weaponAnim = value; }

    private void Start()
    {
        charStats = GetComponent<Character>();
        player = GetComponent<Player>();

        tempAtkSpeed = charStats.AttackSpeed;
        tempAtk = charStats.Attack;

        /*
        if (playerWeapon != null)
        {
            GameObject weapon = Instantiate(playerWeapon.theWeapon, transform);
            WeaponAnim = weapon.GetComponent<Animator>();
            atkPoint = weapon.transform;

            charStats.AttackSpeed -= playerWeapon.attackSpeed;
            charStats.Attack += playerWeapon.bonusAttack;
        }
        */

        for (int i = 0; i < weaponSlot.Length; i++)
        {
            if (weaponSlot[i] != null)
            {
                weapons[i] = weaponSlot[i].theWeapon;
                weapons[i] = Instantiate(weapons[i], transform);
            }
        }

        SetWeapon();
    }

    private void Update()
    {
        ChangeWeapon();
        //if (playerWeapon == null) return;
        Attack();
    }

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            weaponIndex--;
            SetWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            weaponIndex++;
            SetWeapon();
        }

        if (weaponIndex >= weapons.Length - 1)
            weaponIndex = weapons.Length - 1;
        else if (weaponIndex <= 0)
            weaponIndex = 0;
    }

    void SetWeapon()
    {
        if (weaponIndex > weapons.Length - 1 || weaponIndex < 0) return;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponIndex == i)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }
        SelectedWeapon();
    }

    void SelectedWeapon()
    {
        weaponAnim = weapons[weaponIndex].GetComponent<Animator>();
        atkPoint = weapons[weaponIndex].transform;

        charStats.AttackSpeed = tempAtkSpeed;
        charStats.Attack = tempAtk;

        charStats.AttackSpeed -= weaponSlot[weaponIndex].attackSpeed;
        charStats.Attack += weaponSlot[weaponIndex].bonusAttack;

        weaponAnim.SetFloat("moveX", player.tempMove.x);
        weaponAnim.SetFloat("moveY", player.tempMove.y);

        weaponIcon.sprite = weaponSlot[weaponIndex].weaponIcon;
        weaponIcon.color = weaponSlot[weaponIndex].weaponColor;
    }

    void Attack()
    {
        if (charStats.Stamina - weaponSlot[weaponIndex].staminaCost <= 0) return;
        if (Input.GetKeyDown(KeyCode.X) && !charStats.IsAttack)
        {
            charStats.Attacking(WeaponAnim);
            SpawnHitBox();
            StartCoroutine(charStats.DisableAttack(charStats.AttackSpeed));
            charStats.DecreaseStamina(weaponSlot[weaponIndex].staminaCost);
        }
    }

    void SpawnHitBox()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(atkPoint.position, weaponSlot[weaponIndex].radius, targetHit);
        foreach (var hit in col)
        {
            if (hit != null)
            {
                Character enemy = hit.GetComponent<Character>();
                enemy.TakeDamage(charStats.Attack);
                StunEffect(enemy);

                if (enemy.HealthPoint <= 0)
                {
                    levelManager.EnemyCount += 1;
                    charStats.Defense += 1;
                }
            }
        }
    }

    void StunEffect(Character enemy){
        if(weaponSlot[weaponIndex].weaponEffect == Weapon.WeaponEffect.None || 
        (weaponSlot[weaponIndex].weaponEffect != Weapon.WeaponEffect.Stun)) return;

        Debug.Log("Stun Attack!");
        StartCoroutine(enemy.Stun());
    }

    void BurnEffect(){
        
    }

    void BlindEffect(){

    }

    void ChargeAttack()
    {
        if (charStats.Stamina - weaponSlot[weaponIndex].staminaCost <= 0) return;
        if (charStats.IsAttack && Input.GetMouseButton(0))
        {
            Debug.Log("Charge Attack!");
        }
    }
}
