using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Domain;
using Ninject;

namespace Blog4Net.Web.Infrastructure.ModelBinders
{
    public class PostModelBinder : DefaultModelBinder
    {
        private readonly IKernel kernel;

        public PostModelBinder(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var post = (Post)base.BindModel(controllerContext, bindingContext);

            var blogRepository = kernel.Get<IBlogRepository>();

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