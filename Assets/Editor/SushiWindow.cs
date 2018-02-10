using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class SushiWindow : EditorWindow
{
    private static float _space;
    private static bool _opened;
    private static float _span = 20f;
    private static List<SushiDrawer> _sushiDrawers = new List<SushiDrawer>();
    
    private Rect _preRect;

    private static float _heightOfSushi = 31f;
    
    private static Texture _sushiTextureOrigin;
    public static Texture SushiTextureOrigin
    {
        get
        {
            if (_sushiTextureOrigin == null)
            {
		        _sushiTextureOrigin = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Image/sushi.png");
            }
            return _sushiTextureOrigin;
        }
    }

    private static SushiWindow _window;
    
    [MenuItem("SukelerJP/Sushi")]
    public static void Open()
    {
        _window = GetWindow<SushiWindow>();
        _window.minSize = new Vector2(1f,_heightOfSushi);

        _window.name = "SUSHI";
        _opened = true;
        RefreshView(_window.position);
    }

    private void SushiUpdate()
    {
        if (!_opened)
        {
            return;
        }

        foreach (var sushi in _sushiDrawers)
        {
            sushi.UpdateSpace(position);
        }
        _window.Repaint();
        if (_preRect != position)
        {
            _preRect = position;
            RefreshView(_window.position);
        }
    }

    private static void RefreshView(Rect rect)
    {
        if (_sushiDrawers != null)
        {
            _sushiDrawers.Clear();
            _sushiDrawers = new List<SushiDrawer>();
        }
        var laneCount = Mathf.CeilToInt(rect.height / _heightOfSushi);
        for (int i = 0; i < laneCount; i++)
        {
            var drawer = new SushiDrawer
            {
                Speed = 1f,
                StartPos = _span * i,
                SushiTexture = SushiTextureOrigin,
                Width = rect.width,
            };
            _sushiDrawers.Add(drawer);
        }
        
    }
    
    #region EditorWindow
    void Update()
    {
        Debug.Log("update");
        SushiUpdate();
    }

    private void OnDestroy()
    {
        _opened = false;
        _window = null;
    }

    private void OnGUI()
    {
        foreach (var sushiDrawer in _sushiDrawers)
        {
            sushiDrawer.Draw(position);
        }
    }
    #endregion

    
}

public class SushiDrawer
{
    public Texture SushiTexture;
    public float StartPos;
    public float Speed;
    public float Width;
    public int Num;
    public float UnitSpace;

    private const float SushiHeight = 30f;
    private const float SushiWidth = 30f;

    private float _proceeded;
    private float Space
    {
        get
        {
            if (StartPos > Width) StartPos -= Width;
            return StartPos + _proceeded;
        }
    }

    public void UpdateSpace(Rect rect)
    {
        _proceeded += Speed;
        if (Space > rect.width)
        {
            _proceeded = -StartPos;
        }
    }

    public void Init(int unitNum,Rect rect)
    {
        UnitSpace = rect.width / unitNum;
    }

    private float _firstSpace;

    public void Draw()
    {
        _firstSpace += Speed;
        if (_firstSpace >= UnitSpace) _firstSpace = 0f;
        for (int i = 0; i < Num; i++)
        {
            if (i == 0)
            {
                DrawUnit(_firstSpace);
            }
            else
            {
                DrawUnit(UnitSpace);
            }
        }
    }

    private void DrawUnit(float space)
    {
        GUILayout.Space(space);
        GUILayout.Label(SushiTexture,GUILayout.Width(SushiWidth),GUILayout.Height(SushiHeight));
    }
    
    public void Draw(Rect rect)
    {
        if (SushiTexture == null) return;
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(50f);
            GUILayout.Label(SushiTexture,GUILayout.Width(30),GUILayout.Height(30));
            GUILayout.Space(50f);
            GUILayout.Label(SushiTexture,GUILayout.Width(30),GUILayout.Height(30));
            GUILayout.Space(50f);
            GUILayout.Label(SushiTexture,GUILayout.Width(30),GUILayout.Height(30));
//            GUILayout.Space(Space);
//            GUILayout.Label(SushiTexture,GUILayout.Width(30),GUILayout.Height(30));
        }
        GUILayout.Box("", GUILayout.Height(1), GUILayout.Width(rect.width));
    }
}