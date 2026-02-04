using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private float verticalMin = -10f;
    [SerializeField] private float verticalMax = 80f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);
    
    
    private Vector2 lookInput;
    private float rotationX;
    private float rotationY;
    
    void Start()
    {
        Vector3 angle = transform.eulerAngles;
        rotationX = angle.y;
        rotationY = angle.x;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        rotationX += lookInput.x * sensitivity;
        rotationY -= lookInput.y * sensitivity;
        
        Quaternion rotation = Quaternion.Euler(rotationY,rotationX,0);
        transform.rotation = rotation;
        
        rotationY = Mathf.Clamp(rotationY, verticalMin, verticalMax);
        
        Vector3 position = target.position - (transform.forward * distance);
        position = position + (transform.right * offset.x) + (transform.up * offset.y);
        transform.position = position;
    }
}
