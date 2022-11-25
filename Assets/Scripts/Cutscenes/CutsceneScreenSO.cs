using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Cutscene_NAME_Screen_00", menuName = "Game Off/Cutscene Screen")]
public class CutsceneScreenSO : ScriptableObject
{
    [System.Serializable]
    public struct TextAndSound
    {
        public TextAndSound(string text)
        {
            Text = text;
            CharacterSound = default;
            BlockSound = default;
            WaitTime = 0f;
        }

        [TextArea(5, 5)] public string Text;
        public AudioCueSO CharacterSound;
        [Space]
        public AudioCueSO BlockSound;
        public float WaitTime;
    }

    [SerializeField] private CutsceneTestEventSO _testEvent;
    [field: SerializeField] public Sprite Graphic { get; private set; }

    [field: SerializeField]
    public List<TextAndSound> TextBlocks { get; private set; }

    private void OnValidate()
    {
        if (TextBlocks.Count == 0)
        {
            throw new System.Exception($"{name} TextAndSounds can't be empty!");
        }

        if (_testEvent != null)
            _testEvent.Raise(this, name);
    }
}
