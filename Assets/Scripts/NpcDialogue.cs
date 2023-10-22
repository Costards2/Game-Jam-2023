using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcDialogue : MonoBehaviour
{
    public string[] dialogueNpc; 
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI nameNpc;
    public Image imageNpc;
    //public Sprite spriteNpc;

    public bool readyToTalk;
    public bool startDialogue;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && readyToTalk)
        {
            if (!startDialogue)
            {
                //FindObjectOfType<PlayerMovement>().hand = true;
                //FindObjectOfType<PlayerMovement>().axe = false;
                //FindObjectOfType<PlayerMovement>().pickaxe = false;
                //FindObjectOfType<PlayerMovement>().scythe = false;
                FindObjectOfType<PlayerMovement>().speed = 0;
                StartDialogue();
            }
        }
        if (dialogueText.text == dialogueNpc[dialogueIndex] && Input.GetMouseButton(0))
        {
            Debug.Log("PreNext");
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        Debug.Log("PosNext");
        dialogueIndex++;

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
        nameNpc.text = "";
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && FindObjectOfType<PlayerMovement>().hand == true)
        {
            readyToTalk = true;
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
