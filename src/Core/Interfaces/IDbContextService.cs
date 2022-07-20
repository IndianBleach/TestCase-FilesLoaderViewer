using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDbContextService
    {
        Task<T> MakeFirstOrDefaultAsync<T>(string query, object? param = null);
        Task ExecuteAsync(string query, object? param = null);
        Task<IEnumerable<T>> MakeQueryAsync<T>(string query, object? param = null);
    }
}
