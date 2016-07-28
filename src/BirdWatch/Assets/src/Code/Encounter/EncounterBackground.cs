using UnityEngine;
using System.Collections;

public class EncounterBackground : MonoBehaviour
{
    public Sprite MountainSprite;
    public Sprite RedwoodSprite;

    public void SetBackground(Bird.EncounterBg background)
    {
        switch (background)
        {
                case Bird.EncounterBg.Redwood:
                GetComponent<SpriteRenderer>().sprite = RedwoodSprite;
                return;
        }

        GetComponent<SpriteRenderer>().sprite = MountainSprite;
    }
}
