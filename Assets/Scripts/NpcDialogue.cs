using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class NpcDialogue : MonoBehaviour
{
    [Header("Mission Materials")]
    public int missionFibre;
    public int missionStone;
    public int missionWood;

    [Header("Dialogue Mission Imcomplete")]
    public string[] dialogueNpc;
    public int dialogueIndex;

    [Header("Dialogue Mission Completed")]
    public string[] dialogueNpcMissionComplete;
    public int dialogueIndexMissionComplete;

    [Header("NPC Name")]

    public string nameNpcWrite;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameNpc;
    public Image imageNpc;

    [Header("Bools")]
    public bool readyToTalk;
    public bool noWeapon;
    public bool startDialogue;
    public bool missionComplete;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            if (!startDialogue && readyToTalk && noWeapon)
            {
                FindObjectOfType<PlayerMovement>().speed = 0;

                if(missionComplete)
                {
                    StartDialogueMissionColpleted();
                }
                else
                {
                    StartDialogue();
                }
            }
        }

        if (dialogueText.text == dialogueNpc[dialogueIndex] && Input.GetMouseButton(0) && !missionComplete)
        {
            NextDialogue();
        }
        else if (dialogueText.text == dialogueNpcMissionComplete[dialogueIndexMissionComplete] && Input.GetMouseButton(0) && missionComplete)
        {
            NextDialogueMissionColpleted();
        }
    }

    void NextDialogue()
    {
        dialogueIndex = dialogueIndex + 1;

        if(dialogueIndex < dialogueNpc.Length)
        {
            StartCoroutine(ShowDialogue());
        }
        if (dialogueIndex == dialogueNpc.Length)
        {
            readyToTalk = false;
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            FindObjectOfType<PlayerMovement>().speed = 6;
        }
    }

    void StartDialogue()
    {
        nameNpc.text = "" + nameNpcWrite;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = "";

        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void NextDialogueMissionColpleted()
    {
        dialogueIndexMissionComplete++;

        if (dialogueIndexMissionComplete < dialogueNpcMissionComplete.Length)
        {
            StartCoroutine(ShowDialogueMissionColpleted());
        }
        if (dialogueIndexMissionComplete == dialogueNpcMissionComplete.Length)
        {
            readyToTalk = false;
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndexMissionComplete = 0;
            FindObjectOfType<PlayerMovement>().speed = 6;
        }
    }

    void StartDialogueMissionColpleted()
    {
        nameNpc.text = "" + nameNpcWrite;
        startDialogue = true;
        dialogueIndexMissionComplete = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogueMissionColpleted());
    }

    IEnumerator ShowDialogueMissionColpleted()
    {
        dialogueText.text = "";

        foreach (char letter in dialogueNpc[dialogueIndexMissionComplete])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void CheckMission()
    {
        if (FindObjectOfType<PlayerMovement>().wood == missionWood && FindObjectOfType<PlayerMovement>().stone == missionStone && FindObjectOfType<PlayerMovement>().fibre == missionFibre)
        {
            missionComplete = true;
        }
        else
        {
            missionComplete = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckMission();

        if (other.CompareTag("Player"))
        {
            readyToTalk = true;
        }
        else
        {
            readyToTalk = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(FindObjectOfType<PlayerMovement>().hand == true)
        {
            noWeapon = true;
        }
        else
        {
            readyToTalk = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readyToTalk = false;
        }
    }
}
