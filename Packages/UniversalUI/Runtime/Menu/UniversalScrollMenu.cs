using UnityEngine;
using UnityEngine.UI;

namespace Xeon.UniversalUI
{
    public abstract class UniversalScrollMenu : UniversalMenuBase
    {
        [SerializeField]
        protected ScrollRect scrollView;
        [SerializeField, Range(0f, 1f)]
        private float align = 1f;
        protected float Align
        {
            get => align;
            set => align = Mathf.Clamp01(value);
        }

        protected virtual void Move(int move)
        {
            if (!EnableInput || LockInput) return;

            selectableItems[SelectedIndex].UnSelect();
            SelectedIndex += move;
            FixIndex();
            selectableItems[SelectedIndex].Select();
            ReselectCurrentItem();
            FixScroll();
        }

        protected abstract void FixScroll();
    }
}
