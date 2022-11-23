using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "Cutscene_00_Screen_00", menuName = "Game Off/Cutscene Screen")]
public class CutsceneScreenSO : ScriptableObject
{
    [System.Serializable]
    public struct TextAndSound
    {
        [TextArea(4, 4)] public string Text;
        public AudioCueSO CharacterSound;
        [Space]
        public AudioCueSO BlockSound;
        public float WaitTime;
    }

    [field: SerializeField] public Sprite Graphic { get; private set; }

    [field: SerializeField]
    public List<TextAndSound> TextBlocks { get; private set; }

    private void OnValidate()
    {
        if (TextBlocks.Count == 0)
        {
            throw new System.Exception($"{name} TextAndSounds can't be empty!");
        }
    }
}
