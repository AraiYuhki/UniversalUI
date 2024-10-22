namespace Xeon.UniversalUI
{
    public interface IUniversalMenu : IUniversalControllable
    {
        public bool EnableInput { get; set; }
        void Initialize();
        void ReselectCurrentItem();
    }

    public interface IItemAddable<TItem> where TItem : UniversalItemBase
    {
        void AddItem(TItem item, bool changeParent);
    }

    public interface IItemRemovable<TItem> where TItem : UniversalItemBase
    {
        void RemoveItem(TItem item, bool destroy = true);
    }
}
