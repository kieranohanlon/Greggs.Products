using System.Collections.Generic;

namespace Greggs.Products.Infrastructure
{
    public interface IDataAccess<out T>
    {
        IEnumerable<T> List(int? pageStart, int? pageSize);
        int TotalProducts();
    }
}
