using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private TMP_Text _massTextBehind;
    [SerializeField] private TMP_Text _massTextFront;
    [SerializeField] private TMP_Text _winAmountBehind;
    [SerializeField] private TMP_Text _winAmountFront;
    [SerializeField] private Slider _progressSlider;

    [Space]
    public float TempWinValue;

    [Header("Listening to...")]
    [SerializeField] private FloatEventSO _massChangeEvent;

    private void Start()
    {
        _winAmountBehind.text = $"/ {TempWinValue}";
        _winAmountFront.text = $"/ {TempWinValue}";
    }

    private void OnEnable()
    {
        _massChangeEvent.OnEventRaised += ProcessMassChange;
    }

    private void OnDisable()
    {
        _massChangeEvent.OnEventRaised += ProcessMassChange;
    }

    private void ProcessMassChange(float value)
    {
        float rounded = Mathf.Round(value * 100f) / 100f;
        _massTextBehind.text = $"Size: {rounded}";
        _massTextFront.text = $"Size: {rounded}";
        _progressSlider.value = 1 / (TempWinValue / value);
    }
}
