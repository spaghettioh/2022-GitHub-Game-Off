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
        StopAllCoroutines();
        ShowNextScreen();
    }

    public void ShowNextScreen()
    {
        // Unsubscribe now because curtains raise twice for some reason
        _curtainsOpened.OnEventRaised -= ShowNextScreen;

        if (_currentScreenIndex < _screens.Count)
        {
            SetScreenImage(_screens[_currentScreenIndex].Graphic);
            StartCoroutine(ParseNextTextBlockRoutine());
            _currentScreenIndex++;
        }
        else
        {
            _loadEvent.Raise(_nextScene, name);
        }
    }

    private IEnumerator ParseNextTextBlockRoutine()
    {
        _currentScreen = _screens[_currentScreenIndex];

        foreach(var block in _currentScreen.TextBlocks)
        {
            _cutsceneTextBox.text = "";

            if (block.BlockSound != null)
            {
                _audioEvent.RaisePlayback(block.BlockSound);
            }

            foreach (char c in block.Text.ToList())
            {
                yield return new WaitForSeconds(_characterWaitTime);
                _cutsceneTextBox.text += c;

                if (c != ' ')
                {
                    if (block.CharacterSound != null)
                    {
                        _audioEvent.RaisePlayback(block.CharacterSound);
                    }
                    else
                    {
                        _audioEvent.RaisePlayback(_textCharacterAppear);
                    }
                }
            }

            if (block.WaitTime != 0)
            {
                yield return new WaitForSeconds(block.WaitTime);
            }
            else
            {
                yield return new WaitForSeconds(_textWaitTime);
            }
        }

        ShowNextScreen();
    }

    private void SetScreenImage(Sprite sprite)
    {
        _cutsceneThumbnail.sprite = sprite;
    }
}
