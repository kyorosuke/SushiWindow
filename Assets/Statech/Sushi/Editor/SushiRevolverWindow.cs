using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Statech.Editor
{
    public class SushiRevolverWindow : EditorWindow
    {
        private static float _space;
        private static bool _opened;
        private static List<SushiDrawer> _sushiDrawers = new List<SushiDrawer>();
        private static Texture _sushiTextureOrigin;
        private static SushiRevolverWindow _revolverWindow;
    
        private Rect _preRect;
        
        private static Texture SushiTextureOrigin
        {
            get
            {
                if (_sushiTextureOrigin == null)
                {
                    _sushiTextureOrigin = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Statech/Sushi/Image/sushi.png");
                }
                return _sushiTextureOrigin;
            }
        }

    
        [MenuItem("寿司/ください")]
        public static void Open()
        {
            _revolverWindow = GetWindow<SushiRevolverWindow>();
            _revolverWindow.minSize = new Vector2(1f,SushiDrawer.SushiHeight);
            _revolverWindow.titleContent = new GUIContent("へいらっしゃい");
            _opened = true;
            RefreshView(_revolverWindow.position);
        }

        private void SushiUpdate()
        {
            if (!_opened)
            {
                return;
            }

            foreach (var sushi in _sushiDrawers)
            {
                sushi.UpdateSpeed();
            }
            _revolverWindow.Repaint();
            if (_preRect != position)
            {
                _preRect = position;
                RefreshView(_revolverWindow.position);
            }
            
        }

        public static void RefreshFromSetting()
        {
            if (_opened)
            {
                RefreshView(_revolverWindow.position);
            }
        }

        private static void RefreshView(Rect rect)
        {
            if (_sushiDrawers != null)
            {
                _sushiDrawers.Clear();
                _sushiDrawers = new List<SushiDrawer>();
            }
            var laneCount = Mathf.CeilToInt(rect.height / SushiDrawer.SushiHeight);
            var revolveSpeed = EditorPrefs.GetFloat(SushiEditorSetting.SpeedKey,1f);
            if (Math.Abs(revolveSpeed) < 0.01f)
            {
                revolveSpeed = 1f;
            }
            var sushiCount = EditorPrefs.GetInt(SushiEditorSetting.NumKey,1);
            sushiCount -= 1;
            for (int i = 0; i < laneCount; i++)
            {
                var drawer = new SushiDrawer(SushiTextureOrigin, revolveSpeed,sushiCount, _revolverWindow.position);
                if (_sushiDrawers != null)
                {
                    _sushiDrawers.Add(drawer);
                }
            }
            
        }
    
        #region EditorWindow
        void Update()
        {
            SushiUpdate();
        }

        private void OnDestroy()
        {
            _opened = false;
            _revolverWindow = null;
        }

        private void OnGUI()
        {
            if (_sushiDrawers == null || _sushiDrawers.Count == 0 || !_opened)
            {
                //開いたままコンパイル走ると寿司消えちゃうのでその対策
                Open();
            }

            if (_sushiDrawers == null) return; 
            foreach (var sushiDrawer in _sushiDrawers)
            {
                sushiDrawer.Draw();
            }
        }
        #endregion
    }
}