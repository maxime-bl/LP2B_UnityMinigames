using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private int score = 0;
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(int points)
    {
        score += points;
        text.text = "Score : " + score;
    }

    public void Remove(int points)
    {
        score -= points;
        if (score < 0)
        {
            score = 0;
        }
        text.text = "Score : " + score;

    }

    public void ResetScore()
    {
        score = 0;
        text.text = "Score : 0";
    }

    public int GetScore()
    {
        return score;
    }
}
