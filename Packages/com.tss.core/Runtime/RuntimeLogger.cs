using System.Diagnostics;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

namespace TSS.Core
{
    internal static class RuntimeLogger
    {
        [Conditional("TSS_CORE_DEBUG")]
        public static void LogInitialized(IRuntimeLoader loader)
        {
            string name = loader.GetType().Name.Replace("Runtime", "")
                .SplitCamelCase();
            Debug.Log("<color=#ffff00>" + name + " Initialized</color>");
        }
        
        [Conditional("TSS_CORE_DEBUG")]
        public static void LogInitialized()
        {
            Debug.Log("<color=#00ff00>Runtime Initialized</color>");
        }
        
        [Conditional("TSS_CORE_DEBUG")]
        public static void LogDisposed(IRuntimeLoader loader)
        {
            string name = loader.GetType().Name.Replace("Runtime", "")
                .SplitCamelCase();
            Debug.Log("<color=#ffff00>" + name + " Disposed</color>");
        }
        
        [Conditional("TSS_CORE_DEBUG")]
        public static void LogDisposed()
        {
            Debug.Log("<color=#00ff00>Runtime Disposed</color>");
        }
        
        private static string SplitCamelCase(this string camelCaseString)
        {
            if (string.IsNullOrEmpty(camelCaseString)) return camelCaseString;

            string camelCase = Regex.Replace(Regex.Replace(camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            string firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (camelCaseString.Length > 1)
            {
                string rest = camelCase.Substring(1);

                return firstLetter + rest;
            }

            return firstLetter;
        }
    }
}