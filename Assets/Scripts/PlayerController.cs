using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotationSpeed = 5f; 

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
            transform.Translate(direction*speed * Time.deltaTime, Space.World);
            
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
