using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{

    public enum BackgroundType
    {
        Day,
        Night,
        Rain
    }
    
    private FootstepsPlayer _footstepsPlayer;

    private FootstepsPlayer FootstepsPlayer
    {
        get
        {
            if (_footstepsPlayer == null)
            {
                return _footstepsPlayer = GetComponentInChildren<FootstepsPlayer>();
            }
            return _footstepsPlayer;
        }
    }

    private List<ParallaxScroller> _parallaxScrollers;

    public void Awake()
    {
    }

    public void Pause()
    {
        _parallaxScrollers.ForEach(p => p.Pause());

        FootstepsPlayer.Stop();
    }

    public void Resume()
    {
        _parallaxScrollers.ForEach(p => p.Resume());
        FootstepsPlayer.Play();
    }

    public void SetupBackground(GameObject bgPrefab)
    {
        var bg = Instantiate(bgPrefab, transform.position, Quaternion.identity) as GameObject;

        bg.transform.parent = transform;

        _parallaxScrollers = bg.GetComponentsInChildren<ParallaxScroller>().ToList();

        _parallaxScrollers.ForEach(s => s.Setup());
        _parallaxScrollers.ForEach(s => s.Pause());
    }
}
