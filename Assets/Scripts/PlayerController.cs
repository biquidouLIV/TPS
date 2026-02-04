using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float SprintMultiplier = 2;
    [SerializeField] private float rotationSpeed = 5f; 

    float SprintSpeedMultiplier = 1;
    private Vector2 moveInput;
    private Transform CamTransform;

    private Animator animatorRef;
    
    
    
    void Start()
    {
        CamTransform = Camera.main.transform;
        animatorRef = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintSpeedMultiplier = SprintMultiplier;
            animatorRef.SetBool("IsSprinting", true);
        }

        if (context.canceled)
        {
            SprintSpeedMultiplier = 1;
            animatorRef.SetBool("IsSprinting", false);
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 forward = CamTransform.forward;
        Vector3 right = CamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        
        Vector3 direction = (forward * moveInput.y + right * moveInput.x);
        if (direction.magnitude > 0.01f)
        {
            transform.Translate(direction * speed * SprintSpeedMultiplier * Time.deltaTime, Space.World);
            
            Quaternion target_rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * rotationSpeed);
            
            
            animatorRef.SetBool("IsWalking", true);
        }
        else
        {
            animatorRef.SetBool("IsWalking", false);
        }

    }
}
