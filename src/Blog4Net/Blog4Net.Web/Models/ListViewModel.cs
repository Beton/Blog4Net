using System.Collections.Generic;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Domain;

namespace Blog4Net.Web.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository blogRepository, int pageNumber)
        {
            Posts = blogRepository.Posts(pageNumber - 1, 10);
            TotalPosts = blogRepository.TotalPosts();
        }


        public ListViewModel(IBlogRepository blogRepository, string text, string type, int pageNumber)
        {
            switch (type)
            {
                case "Category":
                    Posts = blogRepository.PostsForCategory(text, pageNumber - 1, 10);
                    TotalPosts = blogRepository.TotalPostsForCategory(text);
                    Category = blogRepository.Category(text);
                    break;
                case "Tag":
                    Posts = blogRepository.PostsForTag(text, pageNumber - 1, 10);
                    TotalPosts = blogRepository.TotalPostsForTag(text);
                    Tag = blogRepository.Tag(text);
                    break;
                default:
                    Posts = blogRepository.SearchPosts(text, pageNumber - 1, 10);
                    TotalPosts = blogRepository.TotalSearchPosts(text);
                    break;
            }
        }
       
        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public Category Category { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }
    }
}