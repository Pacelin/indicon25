using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TSS.Utils.Editor
{
    [CustomEditor(typeof(BigExpoLevelingConfig))]
    public class BigExpoLevelingConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var boxSkin = new GUIStyle(GUI.skin.window);
            boxSkin.padding = new RectOffset(4, 4, 4, 4);
            boxSkin.margin = new RectOffset(4, 4, 4, 4);
            var stepLevelSkin = new GUIStyle(GUI.skin.textField);
            stepLevelSkin.margin = new RectOffset(0, 10, 0, 0);
            var labelWithRichSkin = new GUIStyle(GUI.skin.label);
            labelWithRichSkin.richText = true;
            labelWithRichSkin.alignment = TextAnchor.MiddleLeft;
            
            var stepsProp = serializedObject.FindProperty("_steps");
            
            var baseValueProp = serializedObject.FindProperty("_baseValue");
            var data = baseValueProp.FindPropertyRelative("_representation");
            var unpackedData = new int[data.arraySize];
            for (int i = 0; i < unpackedData.Length; i++)
                unpackedData[i] = data.GetArrayElementAtIndex(i).intValue;
            var baseInteger = BigInteger.Parse(unpackedData.ConnectIntoBigInt());
            
            var maxLevelProp = serializedObject.FindProperty("_maxLevel");
            var correctLeveling =
                baseInteger.TryFindMaxLevel(((BigExpoLevelingConfig)serializedObject.targetObject)._steps, out int maxLevel);
            
            EditorGUILayout.BeginVertical(boxSkin);
            stepsProp.arraySize = Mathf.Clamp(EditorGUILayout.IntField("Steps Count", stepsProp.arraySize), 1, 20);
            int[] stepsLevels = new int[stepsProp.arraySize];
            double[] stepsIncrements = new double[stepsProp.arraySize];
            TSSEditorGUI.BeginSmallLabel();
            for (int i = 0; i < stepsProp.arraySize; i++)
            {
                var stepProp = stepsProp.GetArrayElementAtIndex(i);
                var stepIncrementProp = stepProp.FindPropertyRelative("Increment");
                var stepLevelProp = stepProp.FindPropertyRelative("Level");
                if (correctLeveling)
                    stepLevelProp.intValue = Mathf.Clamp(stepLevelProp.intValue, i, maxLevel - stepsProp.arraySize + i);
                else
                    stepLevelProp.intValue = Mathf.Max(stepLevelProp.intValue, i);
                stepsLevels[i] = stepLevelProp.intValue;
                stepsIncrements[i] = stepIncrementProp.doubleValue;
                EditorGUILayout.BeginHorizontal(boxSkin);
                EditorGUILayout.PrefixLabel("Step " + (i + 1));
                EditorGUILayout.PropertyField(stepLevelProp, GUIContent.none, new []{GUILayout.MaxWidth(80)});
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(stepIncrementProp, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            TSSEditorGUI.EndSmallLabel();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(maxLevelProp);
            maxLevelProp.intValue = Mathf.Clamp(maxLevelProp.intValue, 1, maxLevel);
            var maxSelectedLevel = maxLevelProp.intValue;
            if (correctLeveling)
                EditorGUILayout.LabelField("<color=#00ff00>Max Possible Level: " + maxLevel + "</color>", labelWithRichSkin);
            else
                EditorGUILayout.LabelField("<color=#ff0000>Max Possible Level: " + maxLevel + "</color>", labelWithRichSkin);
            
            boxSkin.margin = new RectOffset(4, 4, 4, 14);
            EditorGUILayout.EndVertical();
            boxSkin.margin = new RectOffset(4, 4, 4, 4);

            EditorGUILayout.BeginVertical(boxSkin);

            EditorGUILayout.LabelField("Base Value: <color=#aaaaff>" + 
                baseInteger.ToStringWithLetter(3, null, " {0}") + "</color>", labelWithRichSkin);
            EditorGUILayout.PropertyField(baseValueProp, GUIContent.none);
            EditorGUILayout.EndVertical();

            
            BigInteger curInteger = baseInteger;
            BigInteger maxInteger = baseInteger.ApplyExpo(((BigExpoLevelingConfig)serializedObject.targetObject)._steps, maxSelectedLevel);
            labelWithRichSkin.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.LabelField("<color=#aaaaff>" + maxInteger.ToStringWithLetter() + "</color>", labelWithRichSkin);
            
            var plotRect = EditorGUILayout.GetControlRect();
            plotRect.height = EditorGUIUtility.singleLineHeight * 24;
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 23);
            EditorGUI.DrawRect(plotRect, Color.black);
            if (correctLeveling && maxSelectedLevel > 1)
            {
                plotRect.x += 10;
                plotRect.width -= 22;
                plotRect.y += 10;
                plotRect.height -= 22;
                int currentStep = 0;
                var logMin = Math.Exp(BigInteger.Log(curInteger));
                var logMax = Math.Exp(BigInteger.Log(maxInteger)) - logMin;

                for (int i = 0; i <= maxSelectedLevel; i++)
                {
                    if (currentStep < stepsLevels.Length - 1 &&
                        i >= stepsLevels[currentStep + 1])
                        currentStep++;
                    if (i > 0)
                        curInteger = curInteger * (BigInteger)(stepsIncrements[currentStep] * 100000) / 100000;

                    var logCur = BigInteger.Log(curInteger);
                    var t = (float)((Math.Exp(logCur) - logMin) / logMax);
                    EditorGUI.DrawRect(new Rect(
                        plotRect.x + plotRect.width / maxSelectedLevel * i,
                        plotRect.y + plotRect.height * (1 - t), 2, 2), Color.green);
                }
            }
            labelWithRichSkin.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("<color=#aaaaff>" + baseInteger.ToStringWithLetter() + "</color>", labelWithRichSkin);

            //var incrementProp = serializedObject.FindProperty("_increment");
            /*var maxLevelProp = serializedObject.FindProperty("_maxLevel");
            var maxPossible = baseInteger.FindMaxExpoLevel(unpackedData.Length, incrementProp.doubleValue);
            maxLevelProp.intValue = Mathf.Clamp(maxLevelProp.intValue, 0, maxPossible);
            var levelingPossible = baseInteger.ValidForPower(incrementProp.doubleValue);
            
            EditorGUILayout.PropertyField(incrementProp);
            EditorGUILayout.PropertyField(maxLevelProp);
            EditorGUILayout.LabelField("Max Possible Level: " + maxPossible);
            var style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleLeft;
            style.richText = true;
            if (levelingPossible)
                EditorGUILayout.LabelField("Leveling possible: <color=#00ff00>TRUE</color>", style);
            else
                EditorGUILayout.LabelField("Leveling possible: <color=#ff0000>FALSE</color>", style);*/
            serializedObject.ApplyModifiedProperties();
        }
    }
}