using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TextEffect
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic), typeof(Text), typeof(RectTransform))]
    public abstract class TextEffectBase : UIBehaviour, IMeshModifier
    {
        private List<UIVertex> _stream = new List<UIVertex>();
        private Graphic _textGraphic;
        private RectTransform _rectTransform;

        private Coroutine _setVerticesDirtyCoroutine;

        private Graphic TextGraphic
        {
            get
            {
                return _textGraphic ?? (_textGraphic = GetComponent<Graphic>());
            }
        }

        protected RectTransform RectTransform
        {
            get
            {
                return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
            }
        }

        void IMeshModifier.ModifyMesh(Mesh mesh)
        {
        }

        void IMeshModifier.ModifyMesh(VertexHelper verts)
        {
            _stream.Clear();
            verts.GetUIVertexStream(_stream);

            Modify(ref _stream);

            verts.Clear();
            verts.AddUIVertexTriangleStream(_stream);
        }

        protected static Vector2 GetAnchor(UIVertex[] stream, int index, AlignmentType alignment)
        {
            switch (alignment)
            {
                case AlignmentType.UpperLeft:
                    return GetUpperLeft(stream, index);
                case AlignmentType.UpperCenter:
                    return GetUpperCenter(stream, index);
                case AlignmentType.UpperRight:
                    return GetUpperRight(stream, index);
                case AlignmentType.MiddleLeft:
                    return GetMiddleLeft(stream, index);
                case AlignmentType.MiddleCenter:
                    return GetMiddleCenter(stream, index);
                case AlignmentType.MiddleRight:
                    return GetMiddleRight(stream, index);
                case AlignmentType.LowerLeft:
                    return GetLowerLeft(stream, index);
                case AlignmentType.LowerCenter:
                    return GetLowerCenter(stream, index);
                case AlignmentType.LowerRight:
                    return GetLowerRight(stream, index);
            }
            return default(Vector2);
        }

        /// <summary>
        /// 頂点を変更する
        /// </summary>
        /// <param name="stream"></param>
        protected abstract void Modify(ref List<UIVertex> stream);

        /// <summary>
        /// 頂点のダーティをマークします
        /// </summary>
        protected void SetVerticesDirty()
        {
            if (TextGraphic != null) TextGraphic.SetVerticesDirty();
        }

        protected virtual void BeforAwake()
        {
        }

        protected sealed override void Awake()
        {
            if (TextGraphic != null) TextGraphic.SetVerticesDirty();
            BeforAwake();
        }

        private IEnumerator SetVerticesDirtyAsync()
        {
            yield return null;
            if (TextGraphic != null) TextGraphic.SetVerticesDirty();
        }

        private static Vector2 GetUpperLeft(UIVertex[] stream, int i)
        {
            return stream[i].position;
        }
        private static Vector2 GetUpperCenter(UIVertex[] stream, int i)
        {
            return Vector2.Lerp(stream[i].position, stream[i + 1].position, 0.5f);
        }
        private static Vector2 GetUpperRight(UIVertex[] stream, int i)
        {
            return stream[i + 1].position;
        }
        private static Vector2 GetMiddleLeft(UIVertex[] stream, int i)
        {
            return Vector2.Lerp(stream[i].position, stream[i + 4].position, 0.5f);
        }
        private static Vector2 GetMiddleCenter(UIVertex[] stream, int i)
        {
            return Vector2.Lerp(stream[i].position, stream[i + 3].position, 0.5f);
        }
        private static Vector2 GetMiddleRight(UIVertex[] stream, int i)
        {
            return Vector2.Lerp(stream[i + 1].position, stream[i + 3].position, 0.5f);
        }
        private static Vector2 GetLowerLeft(UIVertex[] stream, int i)
        {
            return stream[i + 4].position;
        }
        private static Vector2 GetLowerCenter(UIVertex[] stream, int i)
        {
            return Vector2.Lerp(stream[i + 3].position, stream[i + 4].position, 0.5f);
        }
        private static Vector2 GetLowerRight(UIVertex[] stream, int i)
        {
            return stream[i + 3].position;
        }
    }

    public enum AlignmentType
    {
        UpperLeft = 0,
        UpperCenter = 1,
        UpperRight = 2,
        MiddleLeft = 10,
        MiddleCenter = 11,
        MiddleRight = 12,
        LowerLeft = 20,
        LowerCenter = 21,
        LowerRight = 22
    }
}