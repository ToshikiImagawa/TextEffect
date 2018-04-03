using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    public class TextBlendScale : TextEffectBase
    {
        [SerializeField, Range(0f, 1f)] private float _blend;
        [SerializeField] private float _interval;
        [SerializeField] private Vector2 _fromScale = Vector2.zero;
        [SerializeField] private Vector2 _toScale = Vector2.zero;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;
        private float _cacheBlend;
        private float _cacheInterval;
        private Vector2 _cacheToScale;
        private Vector2 _cacheFromScale;

        /// <summary>
        /// ブレンド割合
        /// </summary>
        public float Blend
        {
            get { return _blend; }
            set
            {
                _blend = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// インターバル
        /// </summary>
        public float Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// 開始
        /// </summary>
        public Vector2 FromScale
        {
            get { return _fromScale; }
            set
            {
                _fromScale = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        public Vector2 ToScale
        {
            get { return _toScale; }
            set
            {
                _toScale = value;
                SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            FromScale = _fromScale;
            ToScale = _toScale;
            Blend = _blend;
            Interval = _interval;
        }
#endif

        protected override void Modify(ref List<UIVertex> stream)
        {
            var count = 0;
            var streamCount = stream.Count;
            var startPoint = (streamCount / 6 + Interval * 2) * Blend - Interval;
            for (int i = 0; i < streamCount; i += 6)
            {
                var scale = count < startPoint
                    ? ToScale
                    : count > startPoint + Interval
                        ? FromScale
                        : FromScale + (ToScale - FromScale) * (startPoint + Interval - count) / Interval;
                var anchor = GetAnchor(stream.ToArray(), i, _alignment);
                for (var r = 0; r < 6; r++)
                {
                    var element = stream[i + r];

                    var pos = element.position - (Vector3)anchor;

                    Vector2 newPos = new Vector2(pos.x * scale.x, pos.y * scale.y);

                    element.position = newPos + anchor;

                    stream[i + r] = element;
                }
                count++;
            }
        }

        private void Update()
        {
            if (_cacheToScale != ToScale)
            {
                _cacheToScale = ToScale;
                ToScale = _cacheToScale;
            }
            if (_cacheFromScale != FromScale)
            {
                _cacheFromScale = FromScale;
                FromScale = _cacheFromScale;
            }
            if (_cacheBlend != Blend)
            {
                _cacheBlend = Blend;
                Blend = _cacheBlend;
            }
            if (_cacheInterval != Interval)
            {
                _cacheInterval = Interval;
                Interval = _cacheInterval;
            }
        }
    }
}
