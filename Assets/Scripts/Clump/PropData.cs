using UnityEngine;

[System.Serializable]
public class PropData
{
    public Sprite Sprite;
    public float Scale;
    public AudioCueSO PropCollectSound;

    public PropData(Sprite sprite, float scale, AudioCueSO propCollectSound)
    {
        Sprite = sprite;
        Scale = scale;
        PropCollectSound = propCollectSound;
    }
}
