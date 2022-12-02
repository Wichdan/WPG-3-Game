using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] theDoor;
    [SerializeField] int targetEnemy;

    public GameObject[] TheDoor { get => theDoor; set => theDoor = value; }
    public int TargetEnemy { get => targetEnemy; set => targetEnemy = value; }
}
