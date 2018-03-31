using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    [AddComponentMenu("UI/TextEffect/TextScale", 12)]
    public class TextScale : TextEffectBase
    {
        [SerializeField] private Vector2 _scale = Vector2.one;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;
        private Vector2 _cacheScale;

        /// <summary>
        /// 拡大量
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                SetVerticesDirty();
            }
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Scale = _scale;
        }
#endif

        protected override void Modify(ref List<UIVertex> stream)
        {
            for (int i = 0, streamCount = stream.Count; i < streamCount; i += 6)
            {
                var anchor = GetAnchor(stream.ToArray(), i, _alignment);
                for (var r = 0; r < 6; r++)
                {
                    var element = stream[i + r];

                    var pos = element.position - (Vector3)anchor;

                    Vector2 newPos = new Vector2(pos.x * Scale.x, pos.y * Scale.y);

                    element.position = newPos + anchor;

                    stream[i + r] = element;
                }
            }
        }

        private void Update()
        {
            if (_cacheScale != Scale)
            {
                _cacheScale = Scale;
                Scale = _cacheScale;
            }
        }
    }
}