using System.Collections.Generic;

namespace Blog4Net.Core.Domain
{
    public class Category
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string UrlSlug { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<Post> Posts { get; set; }
    }
}