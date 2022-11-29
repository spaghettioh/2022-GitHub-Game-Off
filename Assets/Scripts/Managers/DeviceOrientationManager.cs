using System.Collections;
using UnityEngine;

public class DeviceOrientationManager : MonoBehaviour
{
    [SerializeField] private GameObject _scrim;
    [SerializeField] private GameObject _mobileSpriteWrong;
    [SerializeField] private GameObject _mobileSpriteRight;
    [SerializeField] private float _blinkTime;
    [SerializeField] private AudioEventSO _audioEvent;
    [SerializeField] private VoidEventSO _sceneLoaded;

    [Header("DEBUG ==========")]
    [SerializeField] private bool _isOrientationCorrect;
    [SerializeField] private bool _isShowingScrim;
    [SerializeField] private bool _isSceneLoaded;

    private void OnEnable()
    {
        _sceneLoaded.OnEventRaised += SetSceneLoaded;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= SetSceneLoaded;
    }

    private void SetSceneLoaded() =>_isSceneLoaded = true;

    private void Start()
    {
        _scrim.SetActive(false);
        _mobileSpriteWrong.SetActive(false);
        _mobileSpriteRight.SetActive(false);
    }

    private void Update()
    {
        if (Screen.width < Screen.height && _isSceneLoaded)
        {
            _isOrientationCorrect = false;

            if (!_isShowingScrim)
            {
                StartCoroutine(ShowOrientationRoutine());
            }
        }
        else
        {
            _isOrientationCorrect = true;
        }
    }

    private IEnumerator ShowOrientationRoutine()
    {
        EnableDisableScrim(true);
        while (!_isOrientationCorrect)
        {
            _mobileSpriteWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(_blinkTime);
            _mobileSpriteWrong.SetActive(false);
            _mobileSpriteRight.SetActive(true);
            yield return new WaitForSecondsRealtime(_blinkTime);
            _mobileSpriteRight.SetActive(false);
        }
        EnableDisableScrim(false);
    }

    private void EnableDisableScrim(bool onOff)
    {
        Time.timeScale = onOff ? 0f : 1f;
        _audioEvent.RaisePauseUnpauseMusic(onOff, name);
        _isShowingScrim = onOff;
        _scrim.SetActive(onOff);
    }
}
