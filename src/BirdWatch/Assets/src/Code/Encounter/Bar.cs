using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.src.Code.Encounter
{
    public class Bar
    {
        public enum Type
        {
            Unset,
            Good,
            Ok,
            Bad,
            Clear
        }


        public float Start { get; private set; }
        public float End { get; private set; }
        public Type BarType { get; private set; }

        public Color Color
        {
            get
            {
                switch (BarType)
                {
                    case Type.Good:
                        return Color.green;
                    case Type.Ok:
                        return Color.yellow;
                    case Type.Bad:
                        return Color.red;
                    case Type.Clear:
                        return Color.clear;
                    default:
                        return Color.white;
                }
            }
        }

        public Bar(float start, float end, Type barType)
        {
            Start = start;
            End = end;
            BarType = barType;
        }
    }
}
