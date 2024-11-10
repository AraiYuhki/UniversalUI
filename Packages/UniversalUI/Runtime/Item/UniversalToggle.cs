using System;
using UnityEngine;
using UnityEngine.UI;

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
            set => toggle.interactable = value;
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
    }
}
