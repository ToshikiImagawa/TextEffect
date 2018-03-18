using UnityEngine;
using UnityEngine.EventSystems;

namespace TextEffect.Sample
{

    [RequireComponent(typeof(TextPosition))]
    public class TestScroll : UIBehaviour
    {
        /// <summary>
        /// 速度
        /// </summary>
        [SerializeField] private Vector2 _speed = Vector2.right;

        /// <summary>
        /// 再生判定
        /// </summary>
        public bool IsPlay;

        private TextPosition _textPosition;
        private RectTransform _rectTransform;

        private TextPosition TextPosition
        {
            get
            {
                return _textPosition ?? (_textPosition = GetComponent<TextPosition>());
            }
        }

        private RectTransform RectTransform
        {
            get
            {
                return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
            }
        }

        private void Update()
        {
            if (!IsPlay) return;
            TextPosition.Distance = new Vector2((TextPosition.Distance.x - _speed.x) % RectTransform.rect.width,
                (TextPosition.Distance.y - _speed.y) % RectTransform.rect.height);
        }
    }
}
