using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int targetEnemy;
    [SerializeField] int enemyCount;
    [SerializeField] GameObject[] theDoor;

    private void Update()
    {
        if(theDoor == null) return;
        if (enemyCount >= targetEnemy)
        {
            for (int i = 0; i < theDoor.Length; i++)
            {
                theDoor[i].SetActive(false);
            }
        }
    }

    public GameObject[] TheDoor { get => theDoor; set => theDoor = value; }
    public int EnemyCount { get => enemyCount; set => enemyCount = value; }
    public int TargetEnemy { get => targetEnemy; set => targetEnemy = value; }
    public void ResetMission()
    {
        targetEnemy = 0;
        enemyCount = 0;
        theDoor = null;
    }
}
