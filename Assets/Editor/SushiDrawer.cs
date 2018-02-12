using UnityEngine;

namespace Statech.Editor
{
	public class SushiDrawer
	{
		private Texture _sushiTexture;
		private float _speed;
		private int _num;
		private float _unitSpace;
		private Rect _rect;
		private float _firstSpace;

		public static float SushiHeight = 30f;
		public static float SushiWidth = 30f;
    
		private float SushiSpace
		{
			get { return SushiWidth; }
		}
    

		public SushiDrawer(Texture sushiTexture,float speed,int num,Rect rect)
		{
			_sushiTexture = sushiTexture;
			_speed = speed;
			_num = num;
			_rect = rect;
			_unitSpace = (rect.width - num * SushiSpace ) / (num + 1);
		}

		public void UpdateSpeed()
		{
			_firstSpace += _speed;
			if (_firstSpace >= _unitSpace + SushiWidth)
			{
				_firstSpace = 0f;
			}
		}

		public void Draw()
		{
			using (new GUILayout.HorizontalScope())
			{
				for (int i = 0; i <= _num; i++)
				{
					if (i == 0)
					{
						DrawUnit(_firstSpace);
					}
					else
					{
						DrawUnit(_unitSpace);
					}
				}
			}
			GUILayout.Box("", GUILayout.Height(1), GUILayout.Width(_rect.width) );
		}

		private void DrawUnit(float space)
		{
			GUILayout.Space(space);
			GUILayout.Label(_sushiTexture,GUILayout.Width(SushiWidth),GUILayout.Height(SushiHeight));
		}
	}
}