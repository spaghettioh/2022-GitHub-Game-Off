using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private TMP_Text _propCountText;

    [Space]
    [SerializeField] private int _winAmount;
    [SerializeField] private GameObject _winDialog;
    [SerializeField] private GameObject _loseDialog;

    [Header("Listening to...")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private IntEventSO _propsToWin;
    [SerializeField] private VoidEventSO _winCondition;
    [SerializeField] private VoidEventSO _loseCondition;

    [Header("Broadcasting to...")]
    [SerializeField] private PauseGameplayEventSO _pauseEvent;
    [SerializeField] private LoadEventSO _loadEvent;

    private void OnEnable()
    {
        _propsToWin.OnEventRaised += SetWinAmount;
        _clumpData.OnSizeChanged += UpdateCountText;
        _winCondition.OnEventRaised += ShowWinDialog;
        _loseCondition.OnEventRaised += ShowLoseDialog;
    }

    private void OnDisable()
    {
        _propsToWin.OnEventRaised -= SetWinAmount;
        _clumpData.OnSizeChanged -= UpdateCountText;
        _winCondition.OnEventRaised -= ShowWinDialog;
        _loseCondition.OnEventRaised -= ShowLoseDialog;
    }

    private void Start()
    {
        _winDialog.SetActive(false);
        _loseDialog.SetActive(false);
    }

    private void SetWinAmount(int winAmount)
    {
        _winAmount = winAmount;
        UpdateCountText(0);
    }
    private void UpdateCountText(float f)
    {
        _propCountText.text = $"Collected: {_clumpData.Size} / {_winAmount}";
    }

    private void ShowWinDialog()
    {
        _winDialog.SetActive(true);
    }

    private void ShowLoseDialog()
    {
        _loseDialog.SetActive(true);
    }
}
