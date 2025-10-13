using UnityEngine;

public class CookieMover : MonoBehaviour
{
    public float xSpeed = 1f; // Units per second on X
    public float ySpeed = 0.5f; // Units per second on Y
    public KeyCode keyBind = KeyCode.W;

    void Update()
    {
        if (Input.GetKey(keyBind))
        {
            float xMovement = xSpeed * Time.deltaTime;
            float yMovement = ySpeed * Time.deltaTime;
            float zMovement = ySpeed * Time.deltaTime;

            transform.position += new Vector3(xMovement, 0f, zMovement);
        }
    }
}