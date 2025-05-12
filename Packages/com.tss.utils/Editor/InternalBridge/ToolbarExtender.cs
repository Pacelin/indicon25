using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using Toolbar = UnityEditor.Toolbar;

namespace TSS.Utils.Editor
{
    [InitializeOnLoad]
    public static class ToolbarExtender
    {
        static ToolbarExtender()
        {
            EditorApplication.delayCall += Refresh;
        }

        private static void BuildContainer(
            out VisualElement leftContainerLeft,
            out VisualElement leftContainerRight,
            out VisualElement rightContainerLeft,
            out VisualElement rightContainerRight)
        {
            var rootField = typeof(Toolbar).GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            var root = (VisualElement) rootField.GetValue(Toolbar.get);
            var leftZone = root.Q("ToolbarZoneLeftAlign");
            var rightZone = root.Q("ToolbarZoneRightAlign");
            
            leftContainerLeft = new VisualElement();
            leftContainerLeft.style.flexDirection = FlexDirection.Row;
            leftContainerLeft.style.justifyContent = Justify.FlexStart;
            leftContainerLeft.style.flexGrow = 1;
            
            leftContainerRight = new VisualElement();
            leftContainerRight.style.flexDirection = FlexDirection.Row;
            leftContainerRight.style.justifyContent = Justify.FlexEnd;
            leftContainerRight.style.flexGrow = 1;
            
            rightContainerLeft = new VisualElement();
            rightContainerLeft.style.flexDirection = FlexDirection.Row;
            rightContainerLeft.style.justifyContent = Justify.FlexStart;
            rightContainerLeft.style.flexGrow = 1;
            
            rightContainerRight = new VisualElement();
            rightContainerRight.style.flexDirection = FlexDirection.Row;
            rightContainerRight.style.justifyContent = Justify.FlexEnd;
            rightContainerRight.style.flexGrow = 1;
            
            leftZone.Add(leftContainerLeft);
            leftZone.Add(leftContainerRight);
            rightZone.Add(rightContainerRight);
            rightZone.Add(rightContainerLeft);
        }
        
        private static void Refresh()
        {
            BuildContainer(
                out var ll, 
                out var lr, 
                out var rl, 
                out var rr);

            var llEls = new List<(int order, VisualElement element)>();
            var lrEls = new List<(int order, VisualElement element)>();
            var rlEls = new List<(int order, VisualElement element)>();
            var rrEls = new List<(int order, VisualElement element)>();
            
            foreach (var (type, attribute) in Get())
            {
                if (attribute.ToolbarPanel == EToolbarPanel.LeftPanel)
                {
                    if (attribute.ToolbarAlign == EToolbarAlign.Left)
                        llEls.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                    else
                        lrEls.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                }
                else
                {
                    if (attribute.ToolbarAlign == EToolbarAlign.Left)
                        rlEls.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                    else
                        rrEls.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                }
            }

            foreach (var (_, element) in llEls.OrderBy(t => t.order))
                ll.Add(element);
            foreach (var (_, element) in lrEls.OrderBy(t => t.order))
                lr.Add(element);
            foreach (var (_, element) in rlEls.OrderBy(t => t.order))
                rl.Add(element);
            foreach (var (_, element) in rrEls.OrderBy(t => t.order))
                rr.Add(element);
        }

        private static IEnumerable<(Type type, EditorToolbarElementAttribute Attribute)> Get()
        {
            var types = TypeCache.GetTypesWithAttribute<EditorToolbarElementAttribute>();
            foreach (var type in types)
                if (typeof(VisualElement).IsAssignableFrom(type))
                    yield return (type, type.GetCustomAttribute<EditorToolbarElementAttribute>());
        }
    }
}
