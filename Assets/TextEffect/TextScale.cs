using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    public class TextScale : TextEffectBase
    {
        /// <summary>
        /// 速度
        /// </summary>
        [SerializeField] private float _speed;

        /// <summary>
        /// 最大スケール
        /// </summary>
        [SerializeField] private float _maxScale;

        /// <summary>
        /// 最小スケール
        /// </summary>
        [SerializeField] private float _minScale;


        [SerializeField] private float _scale;

        /// <summary>
        /// 移動量
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                SetVerticesDirty();
            }
        }

        public bool IsPlay;

        private bool _scaleUp;

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
                // 文字の中央を取得（上なら[i+1]）
                var center = Vector2.Lerp(stream[i].position, stream[i + 3].position, 0.5f);
                // 頂点を回す
                for (var r = 0; r < 6; r++)
                {
                    var element = stream[i + r];

                    var pos = element.position - (Vector3) center;

                    Vector2 newPos = pos * Scale;

                    element.position = newPos + center;

                    stream[i + r] = element;
                }
            }
        }

        private void Update()
        {
            if (!IsPlay) return;
            if (_maxScale <= _minScale) return;
            if (_scaleUp)
            {
                var newScale = Scale + _speed;
                if (newScale > _maxScale) _scaleUp = false;
                else Scale = newScale;
            }
            else
            {
                var newScale = Scale - _speed;
                if (newScale < _minScale) _scaleUp = true;
                else Scale = newScale;
            }
        }
    }
}