using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VechicleUse : MonoBehaviour
{
    public bool inCar;
    public bool driving;
    public GameObject target;
    private Vector3 _moveDir = Vector3.zero;
    public float speed = 5;
    private CharacterController _characterController;

    public float RotationSpeed = 240.0f;

    private float Gravity = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inCar && Input.GetKeyDown(KeyCode.E) && !driving)
        {
            driving = true;
            target.transform.GetChild(0).gameObject.SetActive(false);
            target.GetComponent<playerMovement>().enabled = false;
            target.transform.SetParent(transform);
            Camera.main.GetComponent<PlayerFollow>().PlayerTransform = gameObject.transform;
        }
        else if (driving && Input.GetKeyDown(KeyCode.E)) {
            driving = false;
            Camera.main.GetComponent<PlayerFollow>().PlayerTransform = target.transform;
            target.transform.GetChild(0).gameObject.SetActive(true);
            target.GetComponent<playerMovement>().enabled = true;
            target.transform.SetParent(null);
        }

        if (driving) {

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            if (_characterController.isGrounded)
            {
                _moveDir = transform.forward * move.magnitude;
                _moveDir *= speed;
            }

            _moveDir.y -= Gravity * Time.deltaTime;

            _characterController.Move(_moveDir * Time.deltaTime);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            inCar = true;
            target = other.gameObject;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inCar = false;
            target = null;
            driving = false;
            
        }

    }
}
