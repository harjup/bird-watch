using System;
using UnityEngine;
using System.Collections;

public class BirdPhotoResult : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    void Awake()
    {
        _spriteRenderer = transform.FindChild("bird-result-sprite").GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Bird bird)
    {
        var dayString = "-Idle-Day";

        switch (bird.Time)
        {
            case Day.TimeOfDay.Night:
                dayString = "-Night-Sing";
                break;
            case Day.TimeOfDay.Rain:
                dayString = "-Rain-Eat";
                break;
        }


        var sprite = Resources.Load<Sprite>("bird-pictures/" + bird.Id.ToUpper() + dayString);
        _spriteRenderer.sprite = sprite;
    }
}
