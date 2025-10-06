using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    //public GameObject whoWinsHere;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winnerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Corn"))
        {
            if (gameObject.name == "Player2Wins")
            {
                winnerText.text = "player 2 wins!";
                //Debug.Log("Player 2 wins");
            }
            else if (gameObject.name == "Player1Wins")
            {
                winnerText.text = "player 1 wins!";
                //Debug.Log("Player 1 wins");
            }
            
        }
        
        
    }
}
