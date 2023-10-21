using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;

public class ItemContabilizer : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    private int wood = 0;
    private int stone = 0;
    public GameObject player;

    public void IncreaseItemCount()
    {
        wood = player.GetComponent<PlayerMovement>().wood;
        stone = player.GetComponent<PlayerMovement>().stone;
        UpdateItemCountText();
    }

    public void DecreaseItemCount()
    {
        wood = player.GetComponent<PlayerMovement>().wood;
        stone = player.GetComponent<PlayerMovement>().stone;
        UpdateItemCountText();
        
    }

    private void UpdateItemCountText()
    {
        itemText.text = "Wood: " + wood + "\nStone: " + stone;
    }

    void Start()
    {
        UpdateItemCountText();
    }
}
