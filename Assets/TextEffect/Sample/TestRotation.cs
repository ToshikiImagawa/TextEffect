using UnityEngine;
using UnityEngine.EventSystems;

namespace TextEffect.Sample
{
    [RequireComponent(typeof(TextRotation))]
    public class TestRotation : UIBehaviour
    {
        /// <summary>
        /// 速度
        /// </summary>
        [SerializeField] private float _speed;
        public bool IsPlay;

        private TextRotation _textRotation;

        private TextRotation TextRotation
        {
            get
            {
                return _textRotation ?? (_textRotation = GetComponent<TextRotation>());
            }
        }

        private void Update()
        {
            if (!IsPlay) return;
            TextRotation.Angle = (TextRotation.Angle + _speed) % 360f;
        }
    }
}