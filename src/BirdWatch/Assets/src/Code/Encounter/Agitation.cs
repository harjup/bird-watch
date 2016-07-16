using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agitation
{
    private decimal _initalValue;
    public decimal Value { get; private set; }
    public decimal Maximum { get; private set; }

    /// <summary>
    /// Non-inclusive thresold for whether you can take a picture
    /// </summary>
    public decimal CameraThreshold { get; private set;}

    public Agitation(decimal max, decimal cameraThreshold)
    {
        _initalValue = max;
        Maximum = max;

        CameraThreshold = cameraThreshold;

        Reset();
    }

    public void Reset()
    {
        Value = _initalValue;
    }
    
    // TODO: We should enforce the amount only being a whole number or a half-step
    public decimal Increment(decimal amount)
    {
        Value += amount;

        EnforceBounds();

        return Value;
    }

    public decimal Decrement(decimal amount)
    {
        Value -= amount;

        EnforceBounds();

        return Value;
    }


    public bool IsWithinCameraThresold()
    {
        return Value > CameraThreshold;
    }

    public bool IsAtBestValue()
    {
        return Value == Maximum;
    }

    public string GetDescriptionNode()
    {
        var descriptionMap = new Dictionary<int, string>()
        {
            //{3, "AG_GREAT"},
            {2, "AG_OK" },
            {1, "AG_BAD" },
            {0, "AG_LEAVE" },

        };

        var actual = (int)Math.Round(Value, MidpointRounding.AwayFromZero);
        return descriptionMap[actual];
    }

    private void EnforceBounds()
    {
        if (Value > Maximum)
        {
            Value = Maximum;
        }

        if (Value < 0m)
        {
            Value = 0.0m; // The decimal point may or may not matter for equality for some reason.
        }
    }
}
