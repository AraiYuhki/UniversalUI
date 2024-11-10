using System;
using UnityEngine;
using UnityEngine.UI;
using Xeon.XTween;

namespace Xeon.UniversalUI
{
    public class UniversalToggle : UniversalItem
    {
        [SerializeField]
        private Toggle toggle;

        public Action<bool> OnValueChanged { get; set; }
        public override bool Interactable 
        {
            get => toggle.interactable;
            set
            {
                toggle.interactable = value;
                OnChangedEnable();
            }
        }

        public bool Value
        {
            get => toggle.isOn;
            set => toggle.isOn = value;
        }

        private void Awake()
        {
            toggle.onValueChanged.AddListener(value => OnValueChanged?.Invoke(value));
        }
        public override void Submit()
        {
            Value = !Value;
            base.Submit();
        }

        private void OnChangedEnable()
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
            var destColor = Interactable ? normalColor : toggle.colors.disabledColor * normalColor;
            tween = targetImage.TweenColor(destColor, duration).OnComplete(() => tween = null);
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            OnChangedEnable();
        }
    }
}
