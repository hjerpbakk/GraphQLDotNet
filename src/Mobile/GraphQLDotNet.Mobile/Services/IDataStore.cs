using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(DateTime date);
        Task<T> GetItemAsync(DateTime date);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
