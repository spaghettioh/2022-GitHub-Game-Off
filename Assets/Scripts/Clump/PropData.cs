using UnityEngine;

[System.Serializable]
public class PropData
{
    public Sprite Sprite;
    public float Scale;

    public PropData(Sprite sprite, float scale)
    {
        Sprite = sprite;
        Scale = scale;
    }
}
