using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Condition")]
    public bool isInteract;
    public bool isInteracting;

    [Header("Dash")]
    [SerializeField] bool isDodge;
    [SerializeField] Vector2 dodgePos;
    [SerializeField] float dashManaCost;
    [SerializeField] float dodgeSpeed, dodgeCD;
    Vector2 movement;

    [Header("Invincible Anim")]
    [SerializeField] SpriteRenderer sp;
    [SerializeField] Color color;
    Color tempColor;

    [Header("Other")]
    public Vector2 tempMove;

    Rigidbody2D rb;
    Character charStats;
    [Header("Reference")]
    public Transform otherChar;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] LevelManager levelManager;
    [SerializeField] TrailRenderer trailRenderer;
    //[SerializeField] Slider defenseSystem;
    //Cinemachine.CinemachineVirtualCamera cam;
    [SerializeField] Cinemachine.CinemachineConfiner confiner;

    private void Awake()
    {
        charStats = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        charStats.Stamina = charStats.MaxStamina;
        charStats.Mana = charStats.MaxMana;
        //defenseSystem.maxValue = charStats.Defense;

        tempColor = sp.color;
        playerCombat = GetComponent<PlayerCombat>();
        //cam = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    private void Update()
    {
        //defenseSystem.value = charStats.Defense;

        Move();
        Interact();
        WhenInteract();
        Dodge();
        RechargeStamina();
        RechargeMana();

        if (charStats.IsInvincible)
            StartCoroutine(charStats.WhenInvincible(sp, color));
    }

    private void FixedUpdate()
    {
        if (charStats.IsMove && !isDodge)
            rb.MovePosition(rb.position + movement * charStats.MoveSpeed * Time.fixedDeltaTime);
        else if (isDodge && charStats.IsMove){
            rb.MovePosition(rb.position + dodgePos * dodgeSpeed * Time.fixedDeltaTime);
        }
        else if (!charStats.IsMove)
            rb.velocity = new Vector2(0, 0);
    }

    void Move()
    {
        if (!isDodge)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        movement.Normalize();

        if (movement != Vector2.zero)
        {
            charStats.charAnim.SetFloat("moveX", movement.x);
            charStats.charAnim.SetFloat("moveY", movement.y);
            tempMove = movement;

            if (playerCombat.WeaponAnim != null)
            {
                playerCombat.WeaponAnim.SetFloat("moveX", movement.x);
                playerCombat.WeaponAnim.SetFloat("moveY", movement.y);
            }
        }

        charStats.charAnim.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0.01f)
            charStats.IsMove = true;
        else
            charStats.IsMove = false;
    }

    void Interact()
    {
        if(isInteracting) return;
        if (Input.GetKeyDown(KeyCode.Z) && !isInteract)
        {
            isInteract = true;
            StartCoroutine(DisableInteract());
        }
    }

    IEnumerator DisableInteract()
    {
        yield return new WaitForSeconds(0.1f);
        isInteract = false;
    }

    void WhenInteract()
    {
        if (otherChar == null) return;
        float distance = Vector2.Distance(transform.position, otherChar.position);
        if (distance <= 1.2 && isInteract)
        {
            isInteracting = true;
            dialogueManager.StartDialogue();
        }
    }

    void Dodge()
    {
        if (charStats.Stamina - dashManaCost <= 0) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodge && charStats.IsMove)
        {
            isDodge = true;
            dodgePos = movement;

            charStats.Invincible();

            charStats.DecreaseStamina(dashManaCost);
            StartCoroutine(DisableDodge());

            trailRenderer.enabled = true;

            charStats.charAnim.SetBool("isDodge", isDodge);
        }
    }

    IEnumerator DisableDodge()
    {
        yield return new WaitForSeconds(dodgeCD);
        trailRenderer.enabled = false;
        isDodge = false;
        charStats.charAnim.SetBool("isDodge", isDodge);
    }

    void RechargeStamina()
    {
        if (charStats.Stamina >= charStats.MaxStamina)
            charStats.Stamina = charStats.MaxStamina;
        else
            charStats.Stamina += Time.deltaTime;
    }

    void RechargeMana()
    {
        if (charStats.Mana >= charStats.MaxMana)
            charStats.Mana = charStats.MaxMana;
        else
            charStats.Mana += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Interacable")
        {
            otherChar = other.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Interacable")
        {
            dialogueManager.SetDialogue(other.gameObject.GetComponent<DialogueTrigger>().theDialogue);
        }

        if (other.gameObject.layer == 9)
        {
            TeleportTrigger tpTrigger = other.gameObject.GetComponent<TeleportTrigger>();
            transform.position = tpTrigger.playerPos.position;
            //cam.Follow = tpTrigger.camPos;
            tpTrigger.NextStage(confiner);
            levelManager.ResetMission();
        }

        if (other.gameObject.layer == 10)
        {
            MissionTrigger mission = other.gameObject.GetComponent<MissionTrigger>();
            levelManager.TheDoor = mission.TheDoor;
            levelManager.TargetEnemy = mission.TargetEnemy;
            other.gameObject.SetActive(false);
        }
    }
}
