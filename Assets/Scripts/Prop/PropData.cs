using UnityEngine;

[System.Serializable]
public class PropData
{
    public Sprite Sprite;
    public float Scale;
    public int Points;

    public PropData(Sprite sprite, float scale, int scorePoints)
    {
        Sprite = sprite;
        Scale = scale;
        Points = scorePoints;
    }
}
