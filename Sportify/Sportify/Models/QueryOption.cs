using System.Linq.Expressions;

namespace Sportify.Models
{
    public class QueryOption<T> where T : class
    {
        public Expression<Func<T, Object>> OrderBy { get; set; } = null;
        public Expression<Func<T, bool>> Where { get; set; } = null;
    }
}