using System.Collections.Generic;
using UnityEngine;

namespace TextEffect
{
    [AddComponentMenu("UI/TextEffect/TextPosition", 10)]
    public class TextPosition : TextEffectBase
    {
        [SerializeField] private Vector2 _distance = Vector2.zero;
        [SerializeField] private AlignmentType _alignment = AlignmentType.MiddleCenter;
        private Vector2 _cacheDistance;

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector2 Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Distance = _distance;
        }
#endif

        protected override void Modify(ref List<UIVertex> stream)
        {
            var width = RectTransform.rect.width;
            var height = RectTransform.rect.height;
            var pivot = RectTransform.pivot;

            for (int i = 0, streamCount = stream.Count; i < streamCount; i += 6)
            {
                var anchor = GetAnchor(stream.ToArray(), i, _alignment);
                var newCenter = anchor + Distance;
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
                    var pos = element.position - (Vector3)anchor;
                    var newPosition = (Vector2)pos + distanceCenter;
                    element.position = newPosition;

                    stream[i + r] = element;
                }
            }
        }

        private void Update()
        {
            if (_cacheDistance != Distance)
            {
                _cacheDistance = Distance;
                Distance = _cacheDistance;
            }
        }
    }
}