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

        public IList<Post> Posts { get; set; }
        public int TotalPosts { get; set; }
    }
}