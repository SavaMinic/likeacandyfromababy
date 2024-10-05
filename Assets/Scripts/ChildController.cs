using System;
using UnityEngine;

public class ChildController : MonoBehaviour
{

    [SerializeField]
    private float speed;
    
    [SerializeField]
    private float rotationSpeed;
    
    [SerializeField]
    private Transform charTransform;
    
    [SerializeField]
    private Rigidbody charBody;
    
    private Camera _mainCamera;

    private float rotation;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var isKeyPressed = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;
        var inputVector = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        float cameraFacing = _mainCamera.transform.eulerAngles.y;
        var moveVector = Quaternion.Euler(0, cameraFacing, 0) * inputVector;

        var velocity = moveVector * speed;
        charBody.velocity = new Vector3(velocity.x, charBody.velocity.y, velocity.z);

        if (isKeyPressed)
        {
            var rotationStep = rotationSpeed * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(transform.forward, moveVector.normalized, rotationStep, 0.0f);
            charTransform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            Debug.LogError(Input.GetAxis("Horizontal") +" " + Input.GetAxis("Vertical"));
        }
    }
}
