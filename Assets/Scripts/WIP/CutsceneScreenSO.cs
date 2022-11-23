using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Linq;

[CreateAssetMenu(
    fileName = "Cutscene_00_Screen_00", menuName = "Game Off/Cutscene Screen")]
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

    [TextArea(5, 1000)] public string Source;
    [TextArea] public string Splitter;

    [SerializeField] private CutsceneTestEventSO _testEvent;
    [field: SerializeField] public Sprite Graphic { get; private set; }

    [field: SerializeField]
    public List<TextAndSound> TextBlocks { get; private set; }

    private void OnValidate()
    {
        //if (TextBlocks.Count == 0)
        //{
        //    throw new System.Exception($"{name} TextAndSounds can't be empty!");
        //}

        if (Source != "")
        {
            TextBlocks = new List<TextAndSound>();
            Source.Split(Splitter).ToList().ForEach(block =>
            {
                TextBlocks.Add(new TextAndSound(block));
            });
            //Source.ToList().ForEach(c =>
            //{
            //    Debug.Log(c);
            //});
        }

        _testEvent.Raise(this, name);
    }
}
