
using UnityEditor;
using UnityEngine;

namespace Statech.Editor 
{
    public class SushiEditorSetting : EditorWindow
    {
        [SerializeField]
        private int _num;
        [SerializeField]
        private float _speed;

        private const string Prefix = "Statech_";
        public const string NumKey = Prefix + "SushiNum";
        public const string SpeedKey = Prefix + "SushiSpeed";
        
        [MenuItem("寿司/注文")]
        public static void Open()
        {
            var window = GetWindow<SushiEditorSetting>();
            window.Show();
            window.titleContent = new GUIContent("ご注文は");
        }

        private void Save()
        {
            EditorPrefs.SetInt(NumKey,_num);
            EditorPrefs.SetFloat(SpeedKey,_speed);
        }

        private void OnGUI()
        {
            _num = EditorGUILayout.IntSlider("寿司の数", _num,1,20);
            _speed = EditorGUILayout.Slider("回転スピード", _speed,1f,10f);
            if (GUILayout.Button("注文"))
            {
                Save();
                SushiRevolverWindow.RefreshFromSetting();
            }
        }

        private void OnEnable()
        {
            _num = EditorPrefs.GetInt(NumKey);
            _speed = EditorPrefs.GetFloat(SpeedKey);
        }

        private void OnDisable()
        {
            Save();
        }
    }
}
