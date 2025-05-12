using DG.Tweening;
using System;
using TSS.Core.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace TSS.Tweening.Editor
{
    [CustomEditor(typeof(ScriptableTween))]
    public class ScriptableTweenEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var presetProp = serializedObject.FindProperty("_preset");
            var targetsProp = serializedObject.FindProperty("_targets");
            var itemsProp = serializedObject.FindProperty("_items");
            var cacheProp = serializedObject.FindProperty("_cacheTween");
        }
    }

    public static class ScriptableTweenEditorUtils
    {
        
    }
    
    [CustomEditor(typeof(TSSTweener))]
    public class TSSTweenerEditor : UnityEditor.Editor
    {
        private const string ScriptFieldName = "m_Script";

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            //EditorUtils.CreateChildrenPropertyFields(root, serializedObject, ScriptFieldName);

            var tweener = (TSSTweener)serializedObject.targetObject;

            AddButton(root, "Update Tween Properties", () => UpdateTween(tweener));
            AddButton(root, "Reset", () => ResetInspector(tweener));
            AddButton(root, "Complete", () => CompleteInspector(tweener));
            AddButton(root, "Play", () => PlayInspector(tweener));
            
            return root;
        }

        private void AddButton(VisualElement root, string text, Action onClicked)
        {
            var button = new Button
            {
                text = text,
            };
            button.clicked += onClicked;
            root.Add(button);
        }
        
        private void UpdateTween(TSSTweener tweener)
        {
            if (!Application.isPlaying)
                DG.DOTweenEditor.DOTweenEditorPreview.Stop(true);
            tweener.Kill();
            tweener.CreateTween();
        }
        
        private void PlayInspector(TSSTweener tweener)
        {
            ResetInspector(tweener);
            Tween tween = tweener.CreateTween();

            if (!Application.isPlaying)
            {
                DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(tween);
                DG.DOTweenEditor.DOTweenEditorPreview.Start(() => SceneView.lastActiveSceneView.Repaint());
            }
        }

        private void CompleteInspector(TSSTweener tweener)
        {
            tweener.Complete();

            if (!Application.isPlaying)
            {
                DG.DOTweenEditor.DOTweenEditorPreview.Stop();
                SceneView.lastActiveSceneView.Repaint();
            }
        }

        private void ResetInspector(TSSTweener tweener)
        {
            tweener.Reset();

            if (!Application.isPlaying)
            {
                DG.DOTweenEditor.DOTweenEditorPreview.Stop();
                SceneView.lastActiveSceneView.Repaint();
            }
        }
    }
}