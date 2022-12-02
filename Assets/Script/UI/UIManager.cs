using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider staminaSystem;
    [SerializeField] Slider manaSystem, defenseSystem;
    Character player;

    float highHp, medHP, lowHP;
    [SerializeField] Image screenEffect;
    [SerializeField] Animator screenEffectAnim;
    [SerializeField] GameObject pausedUI;
    [SerializeField] bool isPaused;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        
        staminaSystem.maxValue = player.MaxStamina;
        manaSystem.maxValue = player.MaxMana;
        defenseSystem.maxValue = player.Defense;

        highHp = player.HealthPoint * 90 / 100;
        medHP = player.HealthPoint * 50 / 100;
        lowHP = player.HealthPoint * 30 / 100;

        //Debug.Log("High HP: " + highHp + " Med HP: " + medHP + " Low HP: " + lowHP);
    }

    private void Update()
    {
        staminaSystem.value = player.Stamina;
        manaSystem.value = player.Mana;
        defenseSystem.value = player.Defense;
        ChangeScreenEffect();

        if(Input.GetKeyDown(KeyCode.Escape)){
            flipIsPaused();
        }

        if(isPaused)
            PausedGame();
        else
            ContinueGame();
    }

    void ChangeScreenEffect()
    {
        var hp = (float)player.HealthPoint / 100;
        if (hp > highHp)
            screenEffect.enabled = false;
        screenEffectAnim.SetFloat("HP", hp);
    }

    void flipIsPaused() => isPaused = !isPaused;

    void PausedGame(){
        Time.timeScale = 0;
        pausedUI.SetActive(true);
    }

    public void ContinueGame(){
        Time.timeScale = 1;
        pausedUI.SetActive(false);
        isPaused = false;
    }
}
