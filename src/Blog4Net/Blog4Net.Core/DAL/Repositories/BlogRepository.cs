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

        public IList<Post> PostsForTag(string tagSlug, int pageNumber, int pageSize)
        {
            var query = session.Query<Post>()
                               .Where(post => post.Published && post.Tags.Any(t => t.UrlSlug == tagSlug))
                               .OrderByDescending(post => post.Published)
                               .Skip(pageNumber*pageSize)
                               .Take(pageSize)
                               .Fetch(post => post.Category);

            query.FetchMany(post => post.Tags).ToFuture();

            return query.ToFuture().ToList();

        }

        public int TotalPostsForTag(string tagSlug)
        {
            var postCount = session.Query<Post>().Count(post => post.Tags.Any(tag => tag.UrlSlug == tagSlug));

            return postCount;
        }

        public Tag Tag(string tagSlug)
        {
            var tag = session.Query<Tag>().SingleOrDefault(t => t.UrlSlug == tagSlug);

            return tag;
        }

        public IList<Post> SearchPosts(string searchCriteria, int pageNumber, int pageSize)
        {
            var query = session.Query<Post>()
                       .Where(p => p.Published && (p.Title.Contains(searchCriteria) || p.Category.Name.Equals(searchCriteria) || p.Tags.Any(t => t.Name.Equals(searchCriteria))))
                       .OrderByDescending(p => p.PostedOn)
                       .Skip(pageNumber * pageSize)
                       .Take(pageSize)
                       .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();            
        }

        public int TotalSearchPosts(string searchCritera)
        {
            var searchPostsCount = session.Query<Post>().Count(p => p.Published && (p.Title.Contains(searchCritera) || p.Category.Name.Equals(searchCritera) || p.Tags.Any(t => t.Name.Equals(searchCritera))));

            return searchPostsCount;
        }

        public Post Post(int year, int month, string titleSlug)
        {
            var post = session.Query<Post>()
                               .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug == titleSlug)
                               .Fetch(p => p.Category)
                               .FetchMany(p => p.Tags)
                               .FirstOrDefault();

            return post;
        }
    }
}