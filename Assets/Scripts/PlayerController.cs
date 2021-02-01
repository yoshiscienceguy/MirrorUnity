using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator _animator;

    private CharacterController _characterController;

    public float Speed = 5.0f;
    public float runSpeed = 12.0f;
    public float jumpSpeed = 8.0f;
    public float RotationSpeed = 240.0f;
    public float smoothing = 3;
    private float smoothSpeed;
    private float Gravity = 20.0f;

    public float sensitivityX = 270;
    public float sensitivityY = 180;
    private Vector3 _moveDir = Vector3.zero;
    public bool isFPS;
    public bool isGrounded;
    // Use this for initialization
    void Start()
    {
       
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input for axis
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (isFPS)
        {
            _moveDir = new Vector3(h, 0, v);
            _moveDir = transform.TransformDirection(_moveDir);

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            Vector3 mouseMove = new Vector3(mouseY * sensitivityY, mouseX * sensitivityX,0);
            
            transform.Rotate(mouseMove * Time.deltaTime);
            Vector3 nonFixedRotation = transform.eulerAngles;
            nonFixedRotation.z = 0;
            transform.eulerAngles = nonFixedRotation;
        }
        else
        {
            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
            _moveDir = transform.forward * move.magnitude;

        }
        if (_characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                smoothSpeed = Mathf.Lerp(smoothSpeed, runSpeed, Time.deltaTime * smoothing);

            }
            else
            {
                smoothSpeed = Mathf.Lerp(smoothSpeed, Speed, Time.deltaTime * smoothing);

            }
            _moveDir *= smoothSpeed;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                _moveDir.y = jumpSpeed;
            }

        }
        else {
            float oldY = _moveDir.y;
            _moveDir *= smoothSpeed;
            _moveDir.y = oldY;
        }


        _moveDir.y -= Gravity * Time.deltaTime;
        Debug.Log(_moveDir);
        _characterController.Move(_moveDir * Time.deltaTime);

        _animator.SetFloat("speed", _moveDir.magnitude);
    }
}
