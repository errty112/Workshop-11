// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System;
using UnityEngine;
using UnityEngine.Events;

// Note that GameManagerClient inherits from MonoBehaviour, so this is an
// ordinary Unity component.
public class PlayerScore : GameManagerClient
{
    [SerializeField] private UnityEvent<int> onScoreChanged;
    
    // Unity events can simply call this method to increment the player score,
    // even on a prefab without a scene reference to the game manager!
    public void Increment(int amount)
    {
        GameManager.Score += amount;
    }

    public void OnEnable()
    {
        GameManager.OnScoreChanged.AddListener(UpdateScore);
    }
    
    public void OnDisable()
    {
        GameManager.OnScoreChanged.RemoveListener(UpdateScore);
    }

    private void UpdateScore(int score)
    {
        this.onScoreChanged.Invoke(score);
    }

    private void Start()
    {
        GameManager.Score = GameManager.Score; // Invoke event
    }
}
