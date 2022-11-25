using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneTextTestListener : MonoBehaviour
{
    public CutsceneTestEventSO _testEvent;
    public Image Image;
    public TMP_Text Text;

    private void OnValidate()
    {
        _testEvent.OnCutsceneChanged += (screen) =>
        {
            Image.sprite = screen.Graphic;
            Text.text = "";
            screen.TextBlocks.ForEach(block =>
            {
                Text.text += block.Text + "\r\n----------\r\n";
            });
        };
    }
}
