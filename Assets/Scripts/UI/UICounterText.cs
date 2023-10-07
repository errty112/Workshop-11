// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Collections;
using TMPro;
using UnityEngine;

public class UICounterText : MonoBehaviour
{
    [SerializeField] private TMP_Text text; // Text mesh pro component.
    [SerializeField] private string prefix;
    [SerializeField] private int defaultValue;
    [SerializeField] private float lerpSpeed;

    private int _target;

    private void Awake()
    {
        StartCoroutine(Animate());
    }

    public void SetValue(int value)
    {
        this._target = value;
    }

    private IEnumerator Animate()
    {
        float current = this._target = this.defaultValue;
        while (true)
        {
            current = Mathf.Lerp(current, this._target, this.lerpSpeed);
            UpdateText(Mathf.RoundToInt(current));

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void UpdateText(int displayValue)
    {
        this.text.SetText(this.prefix + displayValue);
    }
}
