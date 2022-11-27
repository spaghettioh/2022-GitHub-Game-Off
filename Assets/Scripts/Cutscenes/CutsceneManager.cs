using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Scene setup")]
    [SerializeField] private Image _cutsceneThumbnail;
    [SerializeField] private TMP_Text _cutsceneTextBox;
    [Range(0f, 1f)]
    [SerializeField] private float _characterWaitTime = .1f;
    [Range(0f, 10f)]
    [SerializeField] private float _textWaitTime = 2f;
    [SerializeField] private AudioCueSO _textCharacterAppear;
    [Space]
    [SerializeField] private string _nextScene;

    [Header("Cutscene content")]
    [SerializeField] private List<CutsceneScreenSO> _screens;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _curtainsOpened;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _audioEvent;
    [SerializeField] private LoadEventSO _loadEvent;

    [Header("DEBUG")]
    [SerializeField] private CutsceneScreenSO _currentScreen;
    [SerializeField] private int _currentScreenIndex;
    [SerializeField] private bool _isParsingText;
    [SerializeField] private bool _isWaiting;
    [SerializeField] private bool _isUserRushing;

    private void OnEnable()
    {
        _curtainsOpened.OnEventRaised += ShowNextScreen;
    }

    private void OnDisable()
    {
        _curtainsOpened.OnEventRaised -= ShowNextScreen;
    }

    // Used by a button to skip a block
    public void RushCutscene()
    {
        _isUserRushing = true;
    }

    public void SkipCutscene()
    {
        _loadEvent.Raise(_nextScene, name);
    }

    public void ShowNextScreen()
    {
        // Unsubscribe now because curtains raise twice for some reason
        _curtainsOpened.OnEventRaised -= ShowNextScreen;

        if (_currentScreenIndex < _screens.Count)
        {
            SetScreenImage(_screens[_currentScreenIndex].Graphic);
            StartCoroutine(ParseNextBlockRoutine());
            _currentScreenIndex++;
        }
        else
        {
            _loadEvent.Raise(_nextScene, name);
        }
    }

    private IEnumerator ParseNextBlockRoutine()
    {
        _currentScreen = _screens[_currentScreenIndex];

        foreach (var block in _currentScreen.TextBlocks)
        {
            _cutsceneTextBox.text = "";

            if (block.BlockSound != null)
            {
                _audioEvent.RaisePlayback(block.BlockSound);
            }

            StartCoroutine(ParseNextTextRoutine(block.Text,
                block.CharacterSound, block.WaitTime));

            while (_isParsingText || _isWaiting)
            {
                yield return null;
            }
        }

        ShowNextScreen();
    }

    private IEnumerator ParseNextTextRoutine(
        string text, AudioCueSO characterSound, float waitTime)
    {
        _isParsingText = true;
        foreach (char c in text.ToList())
        {
            if (_isUserRushing)
            {
                _isUserRushing = false;
                _isParsingText = false;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(_characterWaitTime);
                _cutsceneTextBox.text += c;

                if (c != ' ')
                {
                    if (characterSound != null)
                    {
                        _audioEvent.RaisePlayback(characterSound);
                    }
                    else
                    {
                        _audioEvent.RaisePlayback(_textCharacterAppear);
                    }
                }
            }
        }

        StartCoroutine(WaitTimeRoutine(
            waitTime != 0 ? waitTime : _textWaitTime));

        _isParsingText = false;

        while (_isWaiting == true)
        {
            if (_isUserRushing)
            {
                _isWaiting = false;
                _isUserRushing = false;
                _isParsingText = false;
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator WaitTimeRoutine(float waitTime)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        _isWaiting = false;
    }

    private void SetScreenImage(Sprite sprite)
    {
        _cutsceneThumbnail.sprite = sprite;
    }
}
