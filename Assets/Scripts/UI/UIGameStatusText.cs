// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using TMPro;
using UnityEngine;

public class UIGameStatusText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private string[] complements;

    public void GetReady(int waveNumber)
    {
        SetText("Get ready!", "Wave " + waveNumber);
    }

    public void WaveDefeated(int waveNumber)
    {
        SetText(this.complements[(waveNumber - 1) % this.complements.Length],
            "Wave " + waveNumber + " cleared");
    }
    
    public void Win()
    {
        SetText("You won!","That was... anticlimactic?");
    }
    
    public void Lose()
    {
        SetText("You died!","Sorry...");
    }

    public void Clear()
    {
        SetText("");
    }

    private void SetText(string text, string subText = "")
    {
        this.text.SetText(text);
        this.subText.SetText(subText);
    }
}
