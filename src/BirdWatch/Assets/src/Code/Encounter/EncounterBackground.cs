using UnityEngine;
using System.Collections;

public class EncounterBackground : MonoBehaviour
{
    public Sprite MountainSprite;
    public Sprite RedwoodSprite;
    public Sprite RiverSprite;

    public void SetBackground(Bird.EncounterBg background)
    {
        switch (background)
        {
                case Bird.EncounterBg.Redwood:
                GetComponent<SpriteRenderer>().sprite = RedwoodSprite;
                return;
                case Bird.EncounterBg.River:
                GetComponent<SpriteRenderer>().sprite = RiverSprite;
                return;
        }

        GetComponent<SpriteRenderer>().sprite = MountainSprite;
    }
}
