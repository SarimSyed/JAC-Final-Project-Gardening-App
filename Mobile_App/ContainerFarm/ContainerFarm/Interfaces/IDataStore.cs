using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerFarm.Interfaces
{
    interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item); //create operation
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        public ObservableCollection<T> Items { get; } //Item to bind to the collecittion
    }
}
