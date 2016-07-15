﻿using UnityEngine;
using System.Collections;
using System.Linq;
using TreeEditor;

public class SnapshotCameraDisplay : MonoBehaviour
{
    private SpriteRenderer _iconSprite;
    private SpriteRenderer _coneSprite;

    void Start()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        _iconSprite = sprites.First(s => s.name == "camera-icon");
        _coneSprite = sprites.First(s => s.name == "camera-cone");
    }

    public void SetActive()
    {
        _iconSprite.color = Color.white;
        _coneSprite.color = Color.white.SetAlpha(.62f);
    }

    public void SetInactive()
    {
        _iconSprite.color = Color.grey;
        _coneSprite.color = Color.white.SetAlpha(0f);
    }
}