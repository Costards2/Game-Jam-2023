using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemContabilizer : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    private int wood = 0;
    private int stone = 0;
    private int fibre = 0;
    public GameObject player;

    public void IncreaseItemCount()
    {
        wood = player.GetComponent<PlayerMovement>().wood;
        stone = player.GetComponent<PlayerMovement>().stone;
        fibre = player.GetComponent<PlayerMovement>().fibre;
        UpdateItemCountText();
    }

    public void DecreaseItemCount()
    {
        wood = player.GetComponent<PlayerMovement>().wood;
        stone = player.GetComponent<PlayerMovement>().stone;
        stone = player.GetComponent<PlayerMovement>().fibre;
        UpdateItemCountText();
        
    }

    private void UpdateItemCountText()
    {
        itemText.text = "Wood: " + wood + "\nStone: " + stone + "\nFibre: " + fibre;
    }

    void Start()
    {
        UpdateItemCountText();
    }
}
