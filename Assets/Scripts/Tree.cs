using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject wood; 
    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            Instantiate(wood, transform.position, Quaternion.identity);
            Destroy();
        }

    }
    void Destroy()
    {

        Destroy(gameObject);
    }
}
