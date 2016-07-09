using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.src.Code.Encounter;
using DG.Tweening;

public class BreathBar : MonoBehaviour
{
    private GameObject ColoredBox;
    private GameObject FeedbackSprite;
    private GameObject LungBar;
    //TODO: Ugh. What a mess. I feel regretful.

    public List<RatingBar> RatingBars = new List<RatingBar>();

    private void Start()
    {
        ColoredBox = Resources.Load<GameObject>("Prefabs/Battle/ColoredBox");
        FeedbackSprite = Resources.Load<GameObject>("Prefabs/Battle/FloatySprite");


        LungBar = transform.FindChild("lung-bar").gameObject;

        // forgive my tiny array name :<
        float[] mk = { 0f, .1f, .15f, .175f, .825f, .85f, .9f, 1f };
        Color[] colors = { Color.red, Color.yellow, Color.green, Color.clear, Color.green, Color.yellow, Color.red };

        Bar[] bars =
        {
            new Bar(0f, .1f, Bar.Type.Bad),
            new Bar(.1f, .15f, Bar.Type.Ok),
            new Bar(.15f, .175f, Bar.Type.Good),
            new Bar(.175f, .825f, Bar.Type.Clear),
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


    private float speed = .5f;
    private bool prevMouse = false;

    private void Update()
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

            // TODO: not like this, man
            FindObjectOfType<BreathingFace>().Toggle(mouse);

            var lungBar = FindObjectOfType<LungBar>();

            var current = lungBar.RightmostBar();
            var bar = current.Bar;

            var res = Instantiate(FeedbackSprite, current.transform.position, Quaternion.identity) as GameObject;
            switch (bar.BarType)
            {
                case Bar.Type.Good:    
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Good);
                    break;
                case Bar.Type.Ok:
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Ok);
                    break;
                case Bar.Type.Bad:                    
                case Bar.Type.Clear:
                    res.GetComponent<FloatySprite>().SetSprite(FloatySprite.SpriteGraphic.Bad);
                    break;
            }
        }
    }
}
