using UnityEngine;
using System.Collections;

public class Agitation
{
    private decimal _initalValue;
    public decimal Value { get; private set; }
    public decimal Maximum { get; private set; }

    public Agitation(decimal max)
    {
        _initalValue = max;
        Maximum = max;

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
