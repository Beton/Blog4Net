using System.Collections.Generic;
using Blog4Net.Core.Domain;

namespace Blog4Net.Core.DAL.Repositories
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNumber, int pageSize);
        int TotalPosts();
    }
}