using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneTextTestListener : MonoBehaviour
{
    [SerializeField] private CutsceneTestEventSO _testEvent;
    [SerializeField] private Image _thumbnail;
    [SerializeField] private List<TMP_Text> _textBoxes;

    private void OnEnable() => _testEvent.OnCutsceneChanged += UpdateCutscene;
    private void OnDisable() => _testEvent.OnCutsceneChanged -= UpdateCutscene;

    private void UpdateCutscene(CutsceneScreenSO screen)
    {
        // Set the cutscene thumbnail
        _thumbnail.sprite = screen.Graphic;

        // Reset all the boxes
        _textBoxes.ForEach(box =>
        {
            box.gameObject.SetActive(true);
            box.text = "";
            box.gameObject.SetActive(false);
        });

        // Enable & update text boxes for each block
        var index = 0;
        screen.TextBlocks.ForEach(block =>
        {
            var box = _textBoxes[index++];
            box.gameObject.SetActive(true);
            box.text = block.Text;
        });
    }

    private void OnValidate()
    {
        _testEvent.OnCutsceneChanged += UpdateCutscene;
    }
}
