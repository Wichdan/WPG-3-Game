using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    //public Transform camPos;
    public Transform playerPos;
    [SerializeField] PolygonCollider2D polygonCol;

    [SerializeField] GameObject currentStage, nextStage;

    public void NextStage(Cinemachine.CinemachineConfiner confiner)
    {
        currentStage.SetActive(false);
        nextStage.SetActive(true);
        confiner.m_BoundingShape2D = polygonCol;
    }
}
