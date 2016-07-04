using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.src.Code
{
    public static class AwaitCommandFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TypeAndMethod[] FindAwaitableCommands(string method, string target, string[] args)
        {
            var targetActor = GameObject.Find(target);

            if (targetActor == null)
            {
                Debug.LogError("Actor with name '"+ target +"' was not found!");
                return new TypeAndMethod[] {};
            }

            // These are staying split up to make it easier to debug.
            
            var monos = targetActor
                .GetComponents<MonoBehaviour>();

            var types =
                monos.Select(s => s.GetType());

            var methods = types
                .Select(t => new { Type = t, Methods = t.GetMethods() });

            return methods
                .Select(tm =>
                new TypeAndMethod
                (
                    tm.Type,
                    tm.Methods.FirstOrDefault(m =>
                        m.GetCustomAttributes(typeof(AwaitableYarnCommandAttribute), false)
                        .Cast<AwaitableYarnCommandAttribute>()
                        .Any(a => a.AwaitCommandString.Equals(method, StringComparison.InvariantCultureIgnoreCase)))

                ))
                .Where(tm => tm.MethodInfo != null)
                .ToArray();
        }

        public class TypeAndMethod
        {
            public Type Type { get; set; }
            public MethodInfo MethodInfo { get; set; }

            public TypeAndMethod(Type type, MethodInfo methodInfo)
            {
                Type = type;
                MethodInfo = methodInfo;
            }
        }
    }
}
