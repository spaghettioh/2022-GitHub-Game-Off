using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneTester : MonoBehaviour
{
    [SerializeField] private CutsceneTestEventSO _testEvent;
    [SerializeField] private Image _thumbnail;
    [SerializeField] private List<TMP_Text> _textBoxes;

    private void OnEnable() => _testEvent.OnCutsceneChanged += UpdateCutscene;
    private void OnDisable() => _testEvent.OnCutsceneChanged -= UpdateCutscene;

    private void UpdateCutscene(CutsceneScreenSO screen)
    {
        // Reset all the boxes
        _textBoxes.ForEach(box =>
        {
            box.gameObject.SetActive(true);
            box.text = ""; // Unity whines if you update disabled shit
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

        // Set the cutscene thumbnail
        _thumbnail.sprite = screen.Graphic;
    }

    private void OnValidate()
    {
        _testEvent.OnCutsceneChanged += UpdateCutscene;
    }
}
