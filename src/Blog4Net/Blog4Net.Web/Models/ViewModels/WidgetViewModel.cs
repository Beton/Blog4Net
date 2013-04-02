using System.Collections.Generic;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Domain;

namespace Blog4Net.Web.Models.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository blogRepository)
        {
            Categories = blogRepository.Categories();
            Tags = blogRepository.Tags();
            Posts = blogRepository.Posts(1, 10);
        }

        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }
        public IList<Post> Posts { get; set; }
    }
}