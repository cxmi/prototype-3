using TMPro;
using UnityEngine;

public class ScoreKeeping : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ContactLogic.score.ToString();
    }
}
