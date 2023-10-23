using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public GameObject fibre;
    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] private AudioSource Som;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Som.Play();
        if (currentHealth < 0)
        {
            Instantiate(fibre, transform.position, Quaternion.identity);
            Destroy();
            Som.Stop();
        }

    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
