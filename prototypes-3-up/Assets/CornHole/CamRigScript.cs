using UnityEngine;

public class CamRigScript : MonoBehaviour
{
    public Transform player;               // Reference to the player
    public float rotationSmoothSpeed = 5f; // Speed of Y-axis rotation smoothing
    public float positionSmoothSpeed = 10f; // Optional: smoothing for position

    private Vector3 positionOffset;        // Initial offset between rig and player

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to camera rig.");
            enabled = false;
            return;
        }

        // Calculate initial offset from player to rig
        positionOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // STEP 1: Smooth position follow (optional)
        Vector3 targetPosition = player.position + positionOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmoothSpeed * Time.deltaTime);

        // STEP 2: Get only the player's Y rotation
        Quaternion targetRotation = Quaternion.Euler(0f, player.eulerAngles.y, 0f);

        // STEP 3: Smoothly rotate the rig to match player's Y rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}