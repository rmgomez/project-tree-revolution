using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

     
    public class ProgressBar : MonoBehaviour
    {
        private enum Type
        {
            Filled,
            ScaleH,
            ScaleV,
            Slider
        }

        [SerializeField] private Type _type = Type.Filled;
        [SerializeField] private Image _image;
        [SerializeField] private Slider _slider;
        
        public bool Animating { get; private set; }

        private void Awake()
        {
            if (_image == null) _image = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
        
        public Color GetColor ()
        {
            return _image.color;
        }

        public void SetValue01(float value01, bool animate, float time = 0f, bool useRealTime = false)
        {
            var clamped = Mathf.Clamp01(value01);

            if (animate)
            {
                StartCoroutine(SetValueCr(clamped, time, useRealTime));
            }
            else
            {
                SetValue01(clamped);
            }
        }

        private IEnumerator SetValueCr(float value01, float time, bool useRealTime)
        {
            while (Animating)
            {
                yield return null;
            }

            Animating = true;

            var from = GetCurrentValue();
            var to = value01;
            var delay = new Duration(time * Mathf.Abs(from - to), useRealTime);

            while (Animating && !delay.IsDone)
            {
                SetValue01(Mathfx.Hermite(from, to, delay.Progress01));
                yield return null;
            }

            if (Animating)
            {
                SetValue01(to);
            }

            Animating = false;
        }

        public void StopAndReset(float value01)
        {
            StopAllCoroutines();
            Animating = false;
            SetValue01(value01, false, 0f, true);
        }

        private void SetValue01(float value01)
        {
            switch (_type)
            {
                case Type.Filled:     _image.fillAmount = value01;                         break;
                case Type.Slider:     _slider.value = value01;                             break;
                case Type.ScaleH:     transform.localScale = new Vector3(value01, 1f, 1f); break;
                case Type.ScaleV:     transform.localScale = new Vector3(1f, value01, 1f); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public float GetCurrentValue()
        {
            switch (_type)
            {
                case Type.Filled:     return _image.fillAmount;
                case Type.Slider:     return _slider.value;
                case Type.ScaleH:     return transform.localScale.x;
                case Type.ScaleV:     return transform.localScale.y;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

