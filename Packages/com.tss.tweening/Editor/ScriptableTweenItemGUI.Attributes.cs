using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TSS.Utils.Editor;
using UnityEditor;
using UnityEngine;

namespace TSS.Tweening.Editor
{
    public interface IScriptableBox
    {
        float GetHeight();
        void DrawGUI(ref Rect rect);
    }

    public class SingleScriptableBox : IScriptableBox
    {
        private readonly SerializedProperty _property;
        public SingleScriptableBox(SerializedProperty property) =>
            _property = property;

        public float GetHeight() =>
            EditorGUI.GetPropertyHeight(_property, ScriptableTweenItemGUI.GetLabelFor(_property));

        public void DrawGUI(ref Rect rect)
        {
            var label = ScriptableTweenItemGUI.GetLabelFor(_property);
            var propertyRect = new Rect(rect.x + 12, rect.y, rect.width - 12, EditorGUI.GetPropertyHeight(_property, label));
            rect.y += propertyRect.height;
            EditorGUI.PropertyField(propertyRect, _property, label);
        }
    }
    
    public class MultipleScriptableBox : IScriptableBox
    {
        public IList<IScriptableBox> Boxes { get; }

        private readonly string _name;
        public MultipleScriptableBox(string name)
        {
            _name = name;
            Boxes = new List<IScriptableBox>();
        }
        
        public float GetHeight()
        {
            var height = Boxes.Sum(box => box.GetHeight());
            height += (Boxes.Count - 1) * EditorGUIUtility.standardVerticalSpacing;
            height += ScriptableTweenItemGUI.BOX_PADDING * 2;
            if (!_name.StartsWith("!"))
                height += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
            return height;
        }

        public void DrawGUI(ref Rect rect)
        {
            var boxRect = new Rect(rect.x, rect.y, rect.width, GetHeight());
            ScriptableTweenItemGUI.BeginDrawBox(ref boxRect, ref rect);

            if (!_name.StartsWith("!"))
            {
                var labelRect = new Rect(boxRect.x, boxRect.y, boxRect.width, EditorGUIUtility.singleLineHeight);
                boxRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.DrawRect(labelRect, new Color(0, 0, 0, 0.2f));
                EditorGUI.LabelField(labelRect, new GUIContent(_name), new GUIStyle(GUI.skin.label) {padding = new RectOffset(5, 0, 0, 0)});
            }
            
            for (int i = 0; i < Boxes.Count - 1; i++)
            {
                Boxes[i].DrawGUI(ref boxRect);
                boxRect.y += EditorGUIUtility.standardVerticalSpacing;
                rect.y += Boxes[i].GetHeight();
                rect.y += EditorGUIUtility.standardVerticalSpacing;
            }
            Boxes[^1].DrawGUI(ref boxRect);
            rect.y += Boxes[^1].GetHeight();
            ScriptableTweenItemGUI.EndDrawBox(ref rect);
        }
    }
    
    public static partial class ScriptableTweenItemGUI
    {
        public static GUIContent GetLabelFor(SerializedProperty property)
        {
            var attr = property.GetCustomAttribute<LabelAttribute>();
            if (attr == null)
                return new GUIContent(property.displayName);
            return new GUIContent(attr.Label);
        }
        
        public static IList<IScriptableBox> CollectBoxes(SerializedProperty property, Predicate<SerializedProperty> filter)
        {
            var properties = Collect(property, filter);
            var orderedProperties = OrderByAttribute(properties);
            return CompressToBoxes(orderedProperties);
        }
        
        private static IEnumerable<SerializedProperty> Collect(SerializedProperty property, Predicate<SerializedProperty> filter)
        {
            var copy = property.Copy();
            var depth = copy.depth + 1;
            if (!copy.NextVisible(true))
                yield break;

            while (copy.depth == depth)
            {
                if (NeedShowByAttribute(copy) && filter(copy))
                    yield return copy.Copy();
                if (!copy.NextVisible(false))
                    yield break;
            }
        }

        private static IList<IScriptableBox> CompressToBoxes(IEnumerable<SerializedProperty> properties)
        {
            var result = new List<IScriptableBox>();
            var multipleBoxes = new Dictionary<string, MultipleScriptableBox>();

            MultipleScriptableBox GetBox(string path)
            {
                if (!multipleBoxes.ContainsKey(path))
                {
                    string[] split;
                    if (path.Contains("/"))
                        split = path.Split("/");
                    else
                        split = new[] { path };
                    
                    var curPath = "";
                    MultipleScriptableBox curBox = null;
                    foreach (var str in split)
                    {
                        if (curBox == null)
                        {
                            curPath += str;
                            if (!multipleBoxes.ContainsKey(curPath))
                            {
                                multipleBoxes.Add(curPath, new MultipleScriptableBox(str));
                                result.Add(multipleBoxes[curPath]);
                            }
                            curBox = multipleBoxes[curPath];
                        }
                        else
                        {
                            curPath += "/" + str;
                            if (!multipleBoxes.ContainsKey(curPath))
                                multipleBoxes.Add(curPath, new MultipleScriptableBox(str));
                            curBox.Boxes.Add(multipleBoxes[curPath]);
                            curBox = multipleBoxes[curPath];
                        }
                    }
                }

                return multipleBoxes[path];
            }

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<BoxAttribute>();
                if (attribute == null)
                    result.Add(new SingleScriptableBox(property));
                else
                    GetBox(attribute.Name).Boxes.Add(new SingleScriptableBox(property));
            }
            
            return result;
        }

        private static IList<SerializedProperty> OrderByAttribute(IEnumerable<SerializedProperty> properties)
        {
            return properties.OrderBy(p =>
            {
                var attribute = p.GetCustomAttribute<OrderAttribute>();
                if (attribute == null)
                    return 1000;
                return attribute.Order;
            }).ToList();
        }

        private static FieldInfo FindField(Type type, string name)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var field = type.GetField(name, bindingFlags);
            if (field == null && type.BaseType != null)
                return FindField(type.BaseType, name);
            return field;
        }
        private static PropertyInfo FindProperty(Type type, string name)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var field = type.GetProperty(name, bindingFlags);
            if (field == null && type.BaseType != null)
                return FindProperty(type.BaseType, name);
            return field;
        }
        private static MethodInfo FindMethod(Type type, string name)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var field = type.GetMethod(name, bindingFlags);
            if (field == null && type.BaseType != null)
                return FindMethod(type.BaseType, name);
            return field;
        }
        
        private static bool NeedShowByAttribute(SerializedProperty property)
        {
            var showIfAttribute = property.GetCustomAttribute<ShowIfAttribute>();
            if (showIfAttribute == null)
                return true;

            var declaringObj = property.GetDeclaringObject();
            var declaringType = declaringObj.GetType();

            var field = FindField(declaringType, showIfAttribute.ValueProvider);
            if (field != null)
                return (bool)field.GetValue(declaringObj);
            
            var method = FindMethod(declaringType, showIfAttribute.ValueProvider);
            if (method != null)
                return (bool) method.Invoke(declaringObj, System.Array.Empty<object>());

            var prop = FindProperty(declaringType, showIfAttribute.ValueProvider);
            if (prop != null)
                return (bool) prop.GetValue(declaringObj);
            
            return true;
        }

    }
}