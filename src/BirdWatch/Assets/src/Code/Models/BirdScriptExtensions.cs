using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Yarn.Unity;

namespace Assets.src.Code.Models
{
    public static class BirdScriptExtensions
    {
        public static string GetNode(this Bird bird, string node)
        {
            return bird.Id.ToUpper() + "_" + node;
        }

        public static string GetBattleExitNode(this Bird bird, DialogueRunner runner)
        {
            var node = bird.Id.ToUpper() + "_Battle_Exit_" + bird.GetTimeDescription();

            if (runner.NodeExists(node))
            {
                return node;
            }

            Debug.LogError("Unable to find yarn node " + node + ". Looking for backup nodes...");

            var backup = bird.Id.ToUpper() + "_Battle_Exit_Day";
            if (runner.NodeExists(backup))
            {
                return node;
            }

            backup = bird.Id.ToUpper() + "_Battle_Exit_Night";
            if (runner.NodeExists(backup))
            {
                return node;
            }

            backup = bird.Id.ToUpper() + "_Battle_Exit_Rain";
            if (runner.NodeExists(backup))
            {
                return node;
            }

            return node;
        }
    }
}
