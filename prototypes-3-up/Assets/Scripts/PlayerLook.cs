using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float inputSensitivity;

    private float rotationX = 0f;
    private float inputX, inputY;

    private void Awake()
    {
        controls = GetComponent<Controls>();
    }

    void Update()
    {
        //get inputX and inputY based on mouse or right stick movement
        inputX = controls.LookInput().x * inputSensitivity * Time.deltaTime;
        inputY = controls.LookInput().y * inputSensitivity * Time.deltaTime;
        
        //rotate the entire player left and right based on inputX
        transform.Rotate(Vector3.up * inputX);
        
        //rotate the camera up and down based on inputY, clamped to 90 degrees up and down
        rotationX -= inputY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        
        
    }
    
}

