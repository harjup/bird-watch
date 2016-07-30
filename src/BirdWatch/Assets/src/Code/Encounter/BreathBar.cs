using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.src.Code.Encounter;
using DG.Tweening;

public class BreathBar : MonoBehaviour
{
    private GameObject ColoredBox;
    private GameObject FeedbackSprite;
    private GameObject LungBar;
    //TODO: Ugh. What a mess. I feel regretful.

    private AudioSource _breathInSfx;
    private AudioSource _breathOutSfx;
    private AudioSource _coughSfx;

    public List<RatingBar> RatingBars = new List<RatingBar>();

    private int ranking = 0;
    
    public IEnumerator Run(bool isTutorial, Action<decimal> action)
    {
        Init();

        int breaths = 0;
        int breathsMax = 6;
        
        // Press the mouse button!
        LungBar.transform.localScale = LungBar.transform.localScale.SetX(.3f);

        yield return new WaitForSeconds(.05f);

        while (!Input.GetKey(KeyCode.Mouse0))
        {
            yield return null;
        }

        prevMouse = true;
        FindObjectOfType<BreathingFace>().Toggle(prevMouse);
        _breathInSfx.Play();

        while (true)
        {
            breaths = DetectInputAndThenReactToIt(breaths, isTutorial);

            if (breaths >= breathsMax)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(.25f);

        if (ranking > 5 || isTutorial)
        {
            action(1m);
        }
        else if (ranking > 0)
        {
            action(.5m);
        }
        else
        {
            action(-.5m);
        }
        
        Cleanup();
    }

    private void Init()
    {
        var audioSources = GameObject.Find("breath-action").transform.FindChild("sfx").GetComponentsInChildren<AudioSource>();
        _breathInSfx  = audioSources.First(a => a.name == "sfx-breath-in");
        _breathOutSfx = audioSources.First(a => a.name == "sfx-breath-out");
        _coughSfx = audioSources.First(a => a.name == "sfx-breath-cough");

        ranking = 0;

        ColoredBox = Resources.Load<GameObject>("Prefabs/Battle/ColoredBox");
        FeedbackSprite = Resources.Load<GameObject>("Prefabs/Battle/FloatySprite");
        
        LungBar = transform.FindChild("lung-bar").gameObject;

        LungBar.GetComponent<LungBar>().Clear();

        // forgive my tiny array name :<
        //float[] mk = { 0f, .1f, .15f, .175f, .825f, .85f, .9f, 1f };
        //Color[] colors = { Color.red, Color.yellow, Color.green, Color.yellow, Color.clear, Color.yellow, Color.green, Color.yellow, Color.red };

        Bar[] bars =
        {
            new Bar(0f, .1f, Bar.Type.Bad),
            new Bar(.1f, .15f, Bar.Type.Ok),
            new Bar(.15f, .175f, Bar.Type.Good),
            new Bar(.175f, .2f, Bar.Type.Ok),
            new Bar(.2f, .8f, Bar.Type.Clear),
            new Bar(.8f, .825f, Bar.Type.Ok),
            new Bar(.825f, .85f, Bar.Type.Good),
            new Bar(.85f, .9f, Bar.Type.Ok),
            new Bar(.9f, 1f, Bar.Type.Bad),
        };

        foreach (var bar in bars)
        {
            var result = SpawnCollider(bar.Start, bar.End);

            result.GetComponent<SpriteRenderer>().color = bar.Color;

            result.name = bar.BarType.ToString();
            var ratingBar = result.AddComponent<RatingBar>();
            ratingBar.Bar = bar;

            RatingBars.Add(ratingBar);
        }
    }

    private void Cleanup()
    {
        RatingBars.Where(r => r != null).ToList().ForEach(r => Destroy(r.gameObject));

        RatingBars.Clear();
    }


    private GameObject SpawnCollider(float start, float end)
    {
        var startScale = transform.localScale;
        var startpoint = CalculateStartPoint(transform.position, startScale.x, start);
        var newScale = CalculateXScale(startScale.x, (end - start));

        var go = Instantiate(ColoredBox);
        go.transform.localScale = go.transform.localScale.SetX(newScale).SetY(startScale.y);

        go.transform.parent = transform;
        go.transform.position = startpoint;
        go.transform.localPosition = go.transform.localPosition.SetZ(-1);

        return go;
    }

    private Vector3 CalculateStartPoint(Vector3 origin, float xScale, float ratio)
    {
        return origin + Vector3.zero.SetX(1) * ratio * xScale;
    }

    private float CalculateXScale(float xScale, float ratio)
    {
        return xScale * ratio;
    }


    private float speed = .75f;// .5f;
    private bool prevMouse = false;

    private int DetectInputAndThenReactToIt(int breaths, bool isTutorial)
    {
        var x = LungBar.transform.localScale.x;
        var incr = speed * Time.smoothDeltaTime;

        // On mouse change, check where the bar is. Eval.
        var mouse = Input.GetKey(KeyCode.Mouse0);
        if (mouse)
        {
            if (x < 1f)
            {
                LungBar.transform.localScale = LungBar.transform.localScale.SetX(x + incr);
            }
            else
            {
                LungBar.transform.localScale = LungBar.transform.localScale.SetX(1f);
            }
        }
        else
        {
            if (x > 0f)
            {
                LungBar.transform.localScale = LungBar.transform.localScale.SetX(x - incr);
            }
            else
            {
                LungBar.transform.localScale = LungBar.transform.localScale.SetX(0f);
            }
        }

        if (prevMouse != mouse)
        {
            prevMouse = mouse;
            var breathingFace = FindObjectOfType<BreathingFace>();

            breathingFace.Toggle(mouse);

            var lungBar = LungBar.GetComponent<LungBar>();

            var current = lungBar.RightmostBar() ?? RatingBars.First();
            var bar = current.Bar;

            var sourceToPlay = mouse ? _breathInSfx : _breathOutSfx;

            var res = Instantiate(FeedbackSprite, current.transform.position, Quaternion.identity) as GameObject;
            switch (bar.BarType)
            {
                case Bar.Type.Good:    
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Good);
                    sourceToPlay.Play();
                    ranking += 2;
                    break;
                case Bar.Type.Ok:
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Ok);
                    ranking += 1;
                    sourceToPlay.Play();
                    break;
                case Bar.Type.Bad:
                case Bar.Type.Clear:
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Bad);
                    _coughSfx.Play();
                    breathingFace.Shake();
                    ranking -= 2;
                    break;
            }

            // We only want to display the bars that the player's breath bar is heading toward
            if (!prevMouse)
            {
                RatingBars.ForEach(r => r.Show());
                RatingBars.Skip(5).Take(4).ToList().ForEach(r => r.Hide());
            }
            else
            {
                RatingBars.ForEach(r => r.Show());
                RatingBars.Take(4).ToList().ForEach(r => r.Hide());
            }


            // Don't increment on bad if we're doing the tutorial
            if (isTutorial && (bar.BarType == Bar.Type.Bad || bar.BarType == Bar.Type.Clear))
            {
                return breaths;
            }


            return breaths + 1;
        }

        return breaths;
    }
}
