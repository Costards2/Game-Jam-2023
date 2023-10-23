using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class ReceiveItems : MonoBehaviour
{
    [Header("Mission Materials")]
    public int missionFibre;
    public int missionStone;
    public int missionWood;

    [Header("Bools")]
    public bool noWeapon;
    public bool missionComplete;
    public bool canRepair;

    [Header("GameObjects")]
    public GameObject npcMission;
    public GameObject preRepair;
    public GameObject posRepair;

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && canRepair)
        {
            CaculateItems();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRepair = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRepair = false;
        }
    }

    private void CaculateItems()
    {
        if(FindObjectOfType<PlayerMovement>().wood >= missionWood && FindObjectOfType<PlayerMovement>().stone >= missionStone && FindObjectOfType<PlayerMovement>().fibre >= missionFibre)
        {
            missionComplete = true;
            Spend();

        }
    }

    void Spend()
    {
        FindObjectOfType<PlayerMovement>().wood -= missionWood;
        FindObjectOfType<PlayerMovement>().stone -= missionStone;
        FindObjectOfType<PlayerMovement>().fibre -= missionFibre;
        Repair() ;
    }

    void Repair()
    {
        npcMission.GetComponent<NpcDialogue>().npcSatified = missionComplete; 
        preRepair.SetActive(false);
        posRepair.SetActive(true);
    }
}
