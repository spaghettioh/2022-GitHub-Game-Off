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
    [SerializeField] private GameObject _winText;

    [Header("Listening to...")]
    [SerializeField] private FloatEventSO _massChangeEvent;
    [SerializeField] private VoidEventSO _winCondition;

    private void Start()
    {
        _winText.SetActive(false);
    }

    private void OnEnable()
    {
        _massChangeEvent.OnEventRaised += ProcessMassChange;
        _winCondition.OnEventRaised += ShowWinText;
    }

    private void OnDisable()
    {
        _massChangeEvent.OnEventRaised += ProcessMassChange;
        _winCondition.OnEventRaised -= ShowWinText;
    }

    private void ProcessMassChange(float value)
    {
        float rounded = Mathf.Round(value * 100f) / 100f;
        _massTextBehind.text = $"Size: {rounded} / {TempWinValue}";
        _massTextFront.text = $"Size: {rounded} / {TempWinValue}";
        _progressSlider.value = 1 / (TempWinValue / value);

        if (value > TempWinValue)
        {
            ShowWinText();
        }
    }

    private void ShowWinText()
    {
        _winText.SetActive(true);
    }
}
