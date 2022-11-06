using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Mass")]
    [SerializeField] private TMP_Text _massUIText;
    [SerializeField] private FloatEventSO _massChangeEvent;

    private void OnEnable()
    {
        _massChangeEvent.OnEventRaised += UpdateText;
    }

    private void OnDisable()
    {
        _massChangeEvent.OnEventRaised -= UpdateText;
    }

    private void UpdateText(float value)
    {
        _massUIText.text = $"Mass: {value}";
    }
}
