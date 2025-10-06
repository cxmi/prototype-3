using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    void Start()
    {
        //lock mouse and disable cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
