using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HueTester : MonoBehaviour
{
    [SerializeField] private Color _tint;

    private void OnValidate()
    {
        var sprites = FindObjectsOfType<SpriteRenderer>();

        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.color = _tint;
        }

        var images = FindObjectsOfType<Image>();

        foreach (Image image in images)
        {
            image.color = _tint;
        }
    }
}
