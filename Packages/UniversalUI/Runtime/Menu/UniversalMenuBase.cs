using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUniversalMenu
{
    public bool EnableInput { get; set; }
    void Initialize();
    void ReselectCurrentItem();
    void Right();
    void Left();
    void Up();
    void Down();
    void Submit();
}

public interface IItemAddable<TItem> where TItem : UniversalItemBase
{
    void AddItem(TItem item);
}

public interface IITemRemovable<TItem> where TItem : UniversalItemBase
{
    void RemoveItem(TItem item, bool destroy = true);
}

public abstract class UniversalMenuBase
    : MonoBehaviour, IUniversalMenu, IItemAddable<UniversalItemBase>, IITemRemovable<UniversalItemBase>
{
    [SerializeField]
    protected Transform container;
    [SerializeField]
    protected List<UniversalItemBase> items = new();

    public bool EnableInput { get; set; }
    public List<UniversalItemBase> Items => items;

    protected virtual int selectedIndex { get; set; }

    protected Action onSubmit;

    public virtual void Submit()
    {
        if (!EnableInput) return;
        items[selectedIndex].Submit();
    }

    public void AddItem(UniversalItemBase item)
    {
        items.Add(item);
        item.transform.SetParent(container, false);
        item.transform.localScale = Vector3.one;
    }

    public void AddItems(params UniversalItemBase[] items)
    {
        foreach (var item in items) AddItem(item);
    }

    public void RemoveItem(UniversalItemBase item, bool destroy = true)
    {
        items.Remove(item);
        if (destroy) Destroy(item.gameObject);
        ReselectCurrentItem();
    }

    public virtual void Clear(bool destroy = true)
    {
        if (destroy)
        {
            foreach (var item in items) Destroy(item.gameObject);
        }
        items.Clear();
    }

    protected virtual void OnSelected(UniversalItemBase item)
    {
        items[selectedIndex].UnSelect();
        selectedIndex = items.IndexOf(item);
        items[selectedIndex].Select();
    }

    protected virtual void OnSubmit()
    {
        if (!EnableInput) return;
        onSubmit?.Invoke();
    }

    public virtual void Initialize()
    {
        foreach (var item in items)
            item.Initialize(() => OnSelected(item), OnSubmit);
        ReselectCurrentItem();
    }

    protected virtual void FixIndex()
    {
        if (selectedIndex < 0) selectedIndex += items.Count;
        else if (selectedIndex >= items.Count) selectedIndex %= items.Count;
    }

    public virtual void ReselectCurrentItem()
    {
        if (items.Count <= 0) return;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
        foreach (var item in items) item.UnSelect();
        items[selectedIndex].Select();
    }

    public virtual void Right() { }
    public virtual void Left() { }
    public virtual void Up() { }
    public virtual void Down() { }
}
