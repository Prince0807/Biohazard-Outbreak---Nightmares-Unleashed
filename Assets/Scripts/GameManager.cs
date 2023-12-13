using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int _score)
    {
        score += _score;
        GameUIController.Instance.UpdateScore(score);
    }
}
