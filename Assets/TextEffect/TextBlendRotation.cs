using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    public class TextBlendRotation : TextEffectBase
    {
        [SerializeField, Range(0f, 1f)] private float _blend;
        [SerializeField] private float _interval;
        [SerializeField] private float _fromAngle;
        [SerializeField] private float _toAngle;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;

        private float _cacheBlend;
        private float _cacheInterval;
        private float _cacheFromAngle;
        private float _cacheToAngle;

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
        public float FromAngle
        {
            get { return _fromAngle; }
            set
            {
                _fromAngle = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        public float ToAngle
        {
            get { return _toAngle; }
            set
            {
                _toAngle = value;
                SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ToAngle = _toAngle;
            FromAngle = _fromAngle;
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
                var angle = count < startPoint
                    ? ToAngle
                    : count > startPoint + Interval
                        ? FromAngle
                        : FromAngle + (ToAngle - FromAngle) * (startPoint + Interval - count) / Interval;

                var anchor = GetAnchor(stream.ToArray(), i, _alignment);

                for (var r = 0; r < 6; r++)
                {
                    var element = stream[i + r];

                    var pos = element.position - (Vector3) anchor;
                    var newPos = new Vector2(
                        pos.x * Mathf.Cos(angle * Mathf.Deg2Rad) - pos.y * Mathf.Sin(angle * Mathf.Deg2Rad),
                        pos.x * Mathf.Sin(angle * Mathf.Deg2Rad) + pos.y * Mathf.Cos(angle * Mathf.Deg2Rad));
                    element.position = newPos + anchor;

                    stream[i + r] = element;
                }
                count++;
            }
        }

        private void Update()
        {
            if (_cacheToAngle != ToAngle)
            {
                _cacheToAngle = ToAngle;
                ToAngle = _cacheToAngle;
            }
            if (_cacheFromAngle != FromAngle)
            {
                _cacheFromAngle = FromAngle;
                FromAngle = _cacheFromAngle;
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