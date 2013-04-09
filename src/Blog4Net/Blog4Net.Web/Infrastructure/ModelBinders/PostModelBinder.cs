using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Domain;
using Ninject;
using Ninject.Syntax;

namespace Blog4Net.Web.Infrastructure.ModelBinders
{
    public class PostModelBinder : DefaultModelBinder
    {        
        private readonly IBlogRepository blogRepository;

        public PostModelBinder(IKernel kernel)
        {
            this.blogRepository = kernel.Get<IBlogRepository>();
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var post = (Post)base.BindModel(controllerContext, bindingContext);
            
            if (post.Category != null)
                post.Category = blogRepository.Category(post.Category.Id);

            var tags = bindingContext.ValueProvider.GetValue("Tags").AttemptedValue.Split(',');

            if (tags.Length > 0)
            {
                post.Tags = new List<Tag>();

                foreach (var tag in tags)
                {
                    var tmpTag = blogRepository.Tag(int.Parse(tag.Trim()));
                    post.Tags.Add(tmpTag);
                }
            }

            if (bindingContext.ValueProvider.GetValue("oper").AttemptedValue.Equals("edit"))
                post.Modified = DateTime.UtcNow;
            else
                post.PostedOn = DateTime.UtcNow;

            return post;
        }
    }
}