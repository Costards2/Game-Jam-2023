using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stoneAdd = 3;
    public float minHeight = -0.2f;
    public float maxHeight = 1f;
    public float floatSpeed = 0.2f;  
    public float rotationSpeed = 15.0f;
    
    private float initialY;
    //private Vector3 initialPosition;  
    
    private void Start()
    {
        //initialPosition = transform.position;
        initialY = transform.position.y;
    }

    private void Update()
    {
        //float floatOffset = Mathf.Sin(Time.time * floatSpeed);
        //Vector3 newPosition = initialPosition + Vector3.up * floatOffset;
        //transform.position = newPosition;

        float newY = Mathf.PingPong(Time.time * floatSpeed, maxHeight - minHeight) + minHeight;
        transform.position = new Vector3(transform.position.x, newY + initialY, transform.position.z);

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().AddStone(stoneAdd);
            Destroy(gameObject); 
        }
    }
}
