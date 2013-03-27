using System.Collections.Generic;
using System.Linq;
using Blog4Net.Core.Domain;
using NHibernate;
using NHibernate.Linq;

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
            var query =  session.Query<Post>()
                        .Where(p => p.Published)
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                        .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        public int TotalPosts()
        {
            var query = session.QueryOver<Post>().Where(post => post.Published).RowCount();

            return query;
        }

        public IList<Post> PostsForCategory(string categorySlug, int pageNumber, int pageSize)
        {
            var query = session.Query<Post>()
                    .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                    .OrderByDescending(p => p.PostedOn)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            var postCount = session.Query<Post>().Count(post => post.Published && post.Category.UrlSlug == categorySlug);

            return postCount;
        }

        public Category Category(string categorySlug)
        {
            var category = session.Query<Category>().SingleOrDefault(cat => cat.UrlSlug.Equals(categorySlug));

            return category;
        }
    }
}