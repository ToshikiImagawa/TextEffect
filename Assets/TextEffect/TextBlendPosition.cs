using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    public class TextBlendPosition : TextEffectBase
    {
        [SerializeField, Range(0f, 1f)] private float _blend;
        [SerializeField] private float _interval;
        [SerializeField] private Vector2 _fromDistance = Vector2.zero;
        [SerializeField] private Vector2 _toDistance = Vector2.zero;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;
        private float _cacheBlend;
        private float _cacheInterval;
        private Vector2 _cacheToDistance;
        private Vector2 _cacheFromDistance;

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
        public Vector2 FromDistance
        {
            get { return _fromDistance; }
            set
            {
                _fromDistance = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        public Vector2 ToDistance
        {
            get { return _toDistance; }
            set
            {
                _toDistance = value;
                SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            FromDistance = _fromDistance;
            ToDistance = _toDistance;
            Blend = _blend;
            Interval = _interval;
        }
#endif

        protected override void Modify(ref List<UIVertex> stream)
        {
            var width = RectTransform.rect.width;
            var height = RectTransform.rect.height;
            var pivot = RectTransform.pivot;

            var count = 0;
            var streamCount = stream.Count;
            var startPoint = (streamCount / 6 + Interval * 2) * Blend - Interval;

            for (var i = 0; i < streamCount; i += 6)
            {
                var distance = count < startPoint
                    ? ToDistance
                    : count > startPoint + Interval
                        ? FromDistance
                        : FromDistance + (ToDistance - FromDistance) * (startPoint + Interval - count) / Interval;

                var anchor = GetAnchor(stream.ToArray(), i, _alignment);
                var newCenter = anchor + distance;
                var distanceCenter = new Vector2(
                    newCenter.x > 0
                        ? (newCenter.x + width * pivot.x) % width - width * pivot.x
                        : (newCenter.x - width + width * pivot.x) % width + width - width * pivot.x,
                    newCenter.y > 0
                        ? (newCenter.y + height * pivot.y) % height - height * pivot.y
                        : (newCenter.y - height + height * pivot.y) % height + height - height * pivot.y
                );

                for (var r = 0; r < 6; r++)
                {
                    var element = stream[i + r];
                    var pos = element.position - (Vector3) anchor;
                    var newPosition = (Vector2) pos + distanceCenter;
                    element.position = newPosition;

                    stream[i + r] = element;
                }
                count++;
            }
        }

        private void Update()
        {
            if (_cacheToDistance != ToDistance)
            {
                _cacheToDistance = ToDistance;
                ToDistance = _cacheToDistance;
            }
            if (_cacheFromDistance != FromDistance)
            {
                _cacheFromDistance = FromDistance;
                FromDistance = _cacheFromDistance;
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