using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xeon.XTween;

namespace Xeon.UniversalUI
{
    public class UniversalPicker : UniversalItem
    {
        [SerializeField]
        private Color disabledColor = new Color(0.35f, 0.35f, 0.35f, 0.5f);
        [SerializeField]
        private TMP_Text selectedLabel;
        [SerializeField]
        private int selectedIndex = 0;
        [SerializeField]
        private Button rightButton, leftButton;

        [SerializeField]
        private List<string> values = new List<string>();
        [SerializeField]
        private bool isInteractable = true;

        public Action<int> OnValueChanged { get; set; }

        public int SelectedIndex => selectedIndex;

        public override bool Interactable
        {
            get => isInteractable;
            set
            {
                isInteractable = value;
                OnChangedEnable();
            }
        }


        public void SetItems(List<string> values)
        {
            this.values = values;
            FixIndex();
            UpdateView();
        }

        public void Select(int index)
        {
            selectedIndex = index;
            FixIndex();
            UpdateView();
        }

        public void UpdateView() => selectedLabel.text = values[selectedIndex];

        public override void Right() => Move(1);
        public override void Left() => Move(-1);

        private void FixIndex()
        {
            if (selectedIndex < 0)
                selectedIndex += values.Count;
            else if (selectedIndex >= values.Count)
                selectedIndex -= values.Count;
        }

        private void Move(int index)
        {
            selectedIndex += index;
            FixIndex();
            UpdateView();
            OnValueChanged?.Invoke(selectedIndex);
        }

        protected virtual void OnChangedEnable(bool isInstant = false)
        {
            if (rightButton != null)
                rightButton.interactable = isInteractable;
            if (leftButton != null)
                leftButton.interactable = isInteractable;
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            FixIndex();
            UpdateView();
            OnChangedEnable(true);
        }
#endif
    }
}
