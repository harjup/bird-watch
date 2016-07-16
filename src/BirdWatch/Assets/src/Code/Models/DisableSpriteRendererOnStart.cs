using UnityEngine;
using System.Collections;

public class DisableSpriteRendererOnStart : MonoBehaviour
{
    void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }
}
