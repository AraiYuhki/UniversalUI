using System.Collections.Generic;

namespace Xeon.UniversalUI
{
    public interface IDataManageMenu<TData>
    {
        void Initialize(List<TData> data);
        void AddData(TData data);
        void RemoveData(TData data);
    }
}
