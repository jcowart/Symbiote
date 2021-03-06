using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Symbiote.Hibernate
{
    public class SearchCriteria<T> : ISearchCriteria<T>
    {
        private readonly IList<Expression<Func<T, bool>>> _list;
        private int? _pageNumber;
        private int? _pageSize;
        private Queue<Tuple<string, SortOrder>> _orderClause;

        public SearchCriteria()
        {
            _list = new List<Expression<Func<T, bool>>>();
            _orderClause = new Queue<Tuple<string, SortOrder>>();
        }

        #region ISearchCriteria<T> Members

        public ISearchCriteria<T> Add(Expression<Func<T, bool>> expression)
        {
            _list.Add(expression);
            return this;
        }

        public int? PageNumber
        {
            get { return _pageNumber; }
        }

        public int? PageSize
        {
            get { return _pageSize; }
        }

        public IEnumerable<Expression<Func<T, bool>>> All
        {
            get { return _list; }
        }

        public IEnumerable<Tuple<string, SortOrder>> Order
        {
            get
            {
                //while (_orderClause.Count > 0)
                //{
                //    yield return _orderClause.Dequeue();
                //}
                return _orderClause.ToArray();
            }
        }

        public ISearchCriteria<T> PageBy(int pageNumber, int pageSize)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            return this;
        }

        public ISearchCriteria<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> orderBy, SortOrder order)
        {
            _orderClause.Enqueue(Tuple.Create(orderBy.GetMemberName(), order));
            return this;
        }

        #endregion
    }
}