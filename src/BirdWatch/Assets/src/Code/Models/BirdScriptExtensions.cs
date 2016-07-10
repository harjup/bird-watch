using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.Code.Models
{
    public static class BirdScriptExtensions
    {
        public static string GetNode(this Bird bird, string node)
        {
            return bird.Id.ToUpper() + "_" + node;
        }
    }
}
