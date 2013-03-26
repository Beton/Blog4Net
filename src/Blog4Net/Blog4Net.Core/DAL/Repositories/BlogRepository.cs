using System.Collections.Generic;
using System.Linq;
using Blog4Net.Core.Domain;
using NHibernate;

namespace Blog4Net.Core.DAL.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ISession session;

        public BlogRepository(ISession session)
        {
            this.session = session;
        }

        public IList<Post> Posts(int pageNumber, int pageSize)
        {
            var query = session.QueryOver<Post>()
                               .Where(post => post.Published)
                               .OrderBy(post => post.PostedOn).Desc
                               .Fetch(post => post.Category).Eager
                               .Fetch(post => post.Tags).Eager
                               .Skip(pageNumber*pageSize)
                               .Take(pageSize)
                               .Future();

            return query.ToList();
        }

        public int TotalPosts()
        {
            var query = session.QueryOver<Post>().Where(post => post.Published).RowCount();

            return query;
        }
    }
}