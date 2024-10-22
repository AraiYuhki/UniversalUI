using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xeon.XTween;

namespace Xeon.UniversalUI
{
    public class UniversalButton : UniversalItemBase
    {
        [SerializeField]
        protected Button button;
        [SerializeField]
        protected TMP_Text label;

        private bool enable = true;

        public override void AddSubmitEvent(UnityAction action)
        {
            RemoveSubmitEvent(action);
            button.onClick.AddListener(action);
        }

        public override void RemoveSubmitEvent(UnityAction action)
        {
            button.onClick.RemoveListener(action);
        }

        protected override float fadeDuration => button.colors.fadeDuration;
        public bool Interactable
        {
            get => button.interactable;
            set => button.interactable = value;
        }

        public string Label
        {
            get => label.text;
            set => label.text = value;
        }

        protected override void OnEnable()
        {
            var color = isSelected ? selectedColor : normalColor;
            if (!enable)
                color *= button.colors.disabledColor;
            targetImage.color = color;
        }

        protected override void ChangeColor()
        {
            if (tween != null)
            {
                tween?.Kill();
                tween = null;
            }
            var destinationColor = isSelected ? selectedColor : normalColor;
            if (!enable)
                destinationColor *= button.colors.disabledColor;
            tween = targetImage.TweenColor(destinationColor, fadeDuration).OnComplete(() => tween = null);
            tween.SetUseUnscaledTime(true);
        }

        public override void Initialize(Action onSelect = null, Action onSubmit = null)
        {
            button.onClick.AddListener(() =>
            {
                onSubmit?.Invoke();
            });
            base.Initialize(onSelect);
        }

        public override void Submit()
        {
            if (!button.interactable)
                return;
            button.onClick?.Invoke();
        }

        public void Disable()
        {
            enable = false;
            var color = isSelected ? selectedColor : normalColor;
            color *= button.colors.disabledColor;
            targetImage.color = color;
        }
    }
}
