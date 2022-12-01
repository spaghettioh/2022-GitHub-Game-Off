using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Cutscene_NAME_Screen_00",
    menuName = "Game Off/Cutscene Screen")]
public class CutsceneScreenSO : ScriptableObject
{
    [System.Serializable]
    public struct TextAndSound
    {
        [Header("Required")]
        [Tooltip("The text to show."),
            TextArea(5, 5)]
        public string Text;

        [Header("Optional")]
        [Tooltip("Plays when block is shown")]
        public AudioCueSO BlockSound;
        [Tooltip("Custom text character sound")]
        public AudioCueSO CharacterSound;
        [Tooltip("Custom wait time when text is finished")]
        public float WaitTime;

    }

    [field: SerializeField]
    public Sprite Graphic { get; private set; }
    [field: SerializeField]
    public List<TextAndSound> TextBlocks { get; private set; }

    [Tooltip("For use with Cutscene_Tester.scene")]
    [SerializeField]
    private CutsceneTestEventSO _testEvent;
    [Tooltip("Click to fire the tester event")]
    [SerializeField]
    private bool _validateNow;

    //private void OnValidate()
    //{
    //    _validateNow = false;

    //    if (TextBlocks.Count == 0)
    //    {
    //        throw new System.Exception($"{name} TextAndSounds can't be empty!");
    //    }

    //    if (_testEvent != null)
    //        _testEvent.Raise(this, name);
    //}
}
