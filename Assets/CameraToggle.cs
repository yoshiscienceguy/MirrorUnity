using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public bool isFPS;
    public Transform fpsPosition;
    public Transform mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            isFPS = !isFPS;
            
            if (isFPS)
            {

                mainCamera.GetComponent<PlayerFollow>().enabled = false;
                mainCamera.SetParent(fpsPosition);
                mainCamera.localPosition = Vector3.zero;
                mainCamera.localRotation = Quaternion.identity;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                
            }
            else {
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                mainCamera.SetParent(null);
                transform.forward = mainCamera.forward;
                mainCamera.GetComponent<PlayerFollow>().enabled = true;
            }
            transform.GetComponent<PlayerController>().isFPS = isFPS;
        }


    }
}
