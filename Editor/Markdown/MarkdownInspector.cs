using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    [CustomEditor(typeof(TextAsset))]
    internal sealed class MarkdownInspector : Editor
    {
        private TextAsset _target;
        private MarkdownViewer _viewer;
        private Editor _defaultEditor;

        private void OnEnable()
        {
            _target = target as TextAsset;
            string extension = Path.GetExtension(AssetDatabase.GetAssetPath(target)).ToLower();
            if (extension == ".md" || extension == ".markdown")
            {
                GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/HTModuleManager/Editor/Markdown/MarkdownSkin.guiskin");
                _viewer = new MarkdownViewer(skin, AssetDatabase.GetAssetPath(_target), _target.text);
            }
            else
            {
                _defaultEditor = CreateEditor(target, Type.GetType("UnityEditor.TextAssetInspector, UnityEditor"));
            }
        }
        private void OnDestroy()
        {
            if (_defaultEditor != null)
            {
                DestroyImmediate(_defaultEditor);
                _defaultEditor = null;
            }
        }

        public override void OnInspectorGUI()
        {
            if (_viewer != null)
            {
                _viewer.Draw();
            }
            else if (_defaultEditor != null)
            {
                _defaultEditor.OnInspectorGUI();
            }
        }
    }
}