using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xeon.UniversalUI
{
    public abstract class UniversalMenuBase
        : MonoBehaviour, IUniversalMenu, IItemAddable<UniversalItemBase>, IItemRemovable<UniversalItemBase>
    {
        [SerializeField]
        protected Transform container;
        [SerializeField]
        protected List<UniversalItemBase> selectableItems = new();
        [SerializeField]
        protected List<GameObject> allItems = new();

        public bool EnableInput { get; set; }
        public bool LockInput
        {
            get
            {
                if (SelectedIndex < 0 || !EnableInput) return false;
                return selectableItems[SelectedIndex].LockInput;
            }
        }
        public List<GameObject> AllItems => allItems;
        public List<UniversalItemBase> SelectableItems => selectableItems;

        public virtual int SelectedIndex { get; protected set; }

        protected Action onSubmit;
        protected Action onCancel;

        public virtual void Submit()
        {
            if (!EnableInput) return;
            selectableItems[SelectedIndex].Submit();
        }

        public virtual void Cancel()
        {
            if (!EnableInput || LockInput) return;
            onCancel?.Invoke();
        }

        public void AddItem(UniversalItemBase item)
        {
            selectableItems.Add(item);
            allItems.Add(item.gameObject);
            item.transform.SetParent(container, false);
            item.transform.localScale = Vector3.one;
        }

        public void AddUnselectableItem(GameObject gameObject)
        {
            allItems.Add(gameObject);
            gameObject.transform.SetParent(container, false);
            gameObject.transform.localScale = Vector3.one;

        }

        public void AddItems(params UniversalItemBase[] items)
        {
            foreach (var item in items) AddItem(item);
        }

        public void AddUnselectableItems(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects) AddUnselectableItems(gameObject);
        }

        public void RemoveItem(UniversalItemBase item, bool destroy = true)
        {
            selectableItems.Remove(item);
            allItems.Remove(item.gameObject);
            if (destroy) Destroy(item.gameObject);
            ReselectCurrentItem();
        }

        public virtual void Clear(bool destroy = true)
        {
            if (destroy)
            {
                foreach (var item in AllItems) Destroy(item.gameObject);
            }
            AllItems.Clear();
            selectableItems.Clear();
        }

        protected virtual void OnSelected(UniversalItemBase item)
        {
            if (LockInput) return;
            selectableItems[SelectedIndex].UnSelect();
            SelectedIndex = selectableItems.IndexOf(item);
            selectableItems[SelectedIndex].Select();
        }

        protected virtual void OnSubmit()
        {
            if (!EnableInput || LockInput) return;
            onSubmit?.Invoke();
        }

        public virtual void Initialize()
        {
            foreach (var item in selectableItems)
                item.Initialize(() => OnSelected(item), OnSubmit);
            ReselectCurrentItem();
        }

        protected virtual void FixIndex()
        {
            if (SelectedIndex < 0) SelectedIndex += selectableItems.Count;
            else if (SelectedIndex >= selectableItems.Count) SelectedIndex %= selectableItems.Count;
        }

        public virtual void ReselectCurrentItem()
        {
            if (selectableItems.Count <= 0) return;
            SelectedIndex = Mathf.Clamp(SelectedIndex, 0, selectableItems.Count - 1);
            foreach (var item in selectableItems) item.UnSelect();
            selectableItems[SelectedIndex].Select();
        }

        public virtual void Select(int index)
        {
            if (selectableItems.Count <= 0) return;
            SelectedIndex = Mathf.Clamp(index, 0, selectableItems.Count - 1);
            foreach (var item in selectableItems) item.UnSelect();
            selectableItems[SelectedIndex].Select();
        }

        public virtual void Right() { }
        public virtual void Left() { }
        public virtual void Up() { }
        public virtual void Down() { }
    }
}
