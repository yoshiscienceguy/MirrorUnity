using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfGrounded : MonoBehaviour
{
    playerMovement pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            pc.isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
            pc.isGrounded = false;
    }
}
