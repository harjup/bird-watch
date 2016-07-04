using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.Code
{
    public class AwaitableYarnCommandAttribute : Attribute
    {
        public string AwaitCommandString { get; private set; }

        public AwaitableYarnCommandAttribute(string awaitCommandString)
        {
            AwaitCommandString = awaitCommandString;
        }
    }
}
