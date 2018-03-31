using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    [AddComponentMenu("UI/TextEffect/TextRotation", 11)]
    public class TextRotation : TextEffectBase
    {
        [SerializeField] private float _angle;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;
        private float _cacheAngle;

        /// <summary>
        /// 移動量
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Angle = _angle;
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
                    var newPos = new Vector2(
                        pos.x * Mathf.Cos(Angle * Mathf.Deg2Rad) - pos.y * Mathf.Sin(Angle * Mathf.Deg2Rad),
                        pos.x * Mathf.Sin(Angle * Mathf.Deg2Rad) + pos.y * Mathf.Cos(Angle * Mathf.Deg2Rad));
                    element.position = newPos + anchor;

                    stream[i + r] = element;
                }
            }
        }


        private void Update()
        {
            if (_cacheAngle != Angle)
            {
                _cacheAngle = Angle;
                Angle = _cacheAngle;
            }
        }
    }
}