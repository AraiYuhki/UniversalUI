using System;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xeon.XTween;

namespace Xeon.UniversalUI
{
    public class UniversalSlider : UniversalItem
    {
        [SerializeField]
        private Color disabledColor = new Color(0.35f, 0.35f, 0.35f, 0.5f);
        [SerializeField]
        private TMP_InputField input;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Button rightButton, leftButton;
        [SerializeField]
        private float minValue = 0f;
        [SerializeField]
        private float maxValue = 100f;
        [SerializeField]
        private float step = 1f;
        [SerializeField]
        private bool isInterger = true;
        [SerializeField]
        private bool isInteractable = true;

        private bool isValueChanging = false;

        public Action<float> OnValueChanged { get; set; }
        public UnityAction OnSubmit
        {
            get => onSubmit;
            set => onSubmit = value;
        }

        public float Value
        {
            get => slider.value;
            set => slider.value = value;
        }

        public void SetMin(float min)
            => slider.minValue = min;

        public void SetMax(float max)
            => slider.maxValue = max;

        public void SetData(float value, float min, float max)
        {
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = value;
        }

        public void SetData(float value, float max)
        {
            slider.maxValue = max;
            slider.value = value;
        }

        public void SetStep(float step) => this.step = step;
        public void SetInterger(bool flag) => this.isInterger = flag;

        public int IntValue => Mathf.FloorToInt(slider.value);

        public override bool Interactable 
        {
            get => isInteractable;
            set 
            {
                isInteractable = value;
                if (slider != null)
                    slider.interactable = value;
                if (input != null)
                    input.interactable = value;
                if (rightButton != null)
                    rightButton.interactable = value;
                if (leftButton != null)
                    leftButton.interactable = value;
                OnChangedEnable();
            }
        }

        private void Awake()
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener(OnSliderValueChanged);

            input.text = slider.value.ToString();
            input.onValueChanged.AddListener(OnInputValueChanged);
        }

        protected virtual void OnSliderValueChanged(float value)
        {
            if (isValueChanging) return;
            isValueChanging = true;
            var newValue = isInterger ? Mathf.FloorToInt(value) : value;
            input.text = newValue.ToString();
            OnValueChanged?.Invoke(newValue);
            isValueChanging = false;
        }

        protected virtual void OnInputValueChanged(string value)
        {
            if (isValueChanging) return;
            isValueChanging = true;
            if (float.TryParse(value, out var result))
                slider.value = isInterger ? Mathf.FloorToInt(result) : result;
            OnValueChanged?.Invoke(slider.value);
            isValueChanging = false;
        }

        public override void Right() => slider.value += step;

        public override void Left() => slider.value -= step;

        protected virtual void OnChangedEnable(bool isInstant = false)
        {
            if (isInstant)
            {
                targetImage.color = isInteractable ? normalColor : disabledColor;
                return;
            }
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
            tween = targetImage.TweenColor(isInteractable ? normalColor : disabledColor, duration).OnComplete(() => tween = null);
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            if (slider != null)
                slider.interactable = isInteractable;
            if (input != null)
                input.interactable = isInteractable;
            if (rightButton != null)
                rightButton.interactable = isInteractable;
            if (leftButton != null)
                leftButton.interactable = isInteractable;
            OnChangedEnable(true);
        }
    }
}
