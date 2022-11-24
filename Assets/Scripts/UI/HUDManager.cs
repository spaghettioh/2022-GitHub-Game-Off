using TMPro;
using UnityEngine;

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
        _clumpData.OnPropCountChanged += UpdateCountText;
        _winCondition.OnEventRaised += ShowWinDialog;
        _loseCondition.OnEventRaised += ShowLoseDialog;
    }

    private void OnDisable()
    {
        _propsToWin.OnEventRaised -= SetWinAmount;
        _clumpData.OnPropCountChanged -= UpdateCountText;
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
        UpdateCountText();
    }
    private void UpdateCountText(int count = 0)
    {
        _propCountText.text = $"Collected: {count} / {_winAmount}";
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
