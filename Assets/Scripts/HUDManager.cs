using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private TMP_Text _sizeTextBehind;
    [SerializeField] private TMP_Text _sizeTextFront;
    [SerializeField] private Slider _progressSlider;

    [Space]
    [SerializeField] private float _tempWinValue;
    [SerializeField] private GameObject _winDialog;

    [Header("Listening to...")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private VoidEventSO _winCondition;

    private void Start()
    {
        _winDialog.SetActive(false);
    }

    private void OnEnable()
    {
        _clumpData.OnSizeChanged += ProcessSizeChange;
        _winCondition.OnEventRaised += CheckForWin;
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -= ProcessSizeChange;
        _winCondition.OnEventRaised -= CheckForWin;
    }

    private void ProcessSizeChange(float size)
    {
        float rounded = Mathf.Round(size * 100f) / 100f;
        string currentSize = $"Size: {rounded} / {_tempWinValue}";
        _sizeTextBehind.text = currentSize;
        _sizeTextFront.text = currentSize;
        _progressSlider.value = 1 / (_tempWinValue / size);

        CheckForWin();
    }

    private void CheckForWin()
    {
        if (_clumpData.SizeInMeters >= _tempWinValue)
        {
            _winDialog.SetActive(true);
        }
    }
}
