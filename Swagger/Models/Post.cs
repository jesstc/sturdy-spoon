using System;
using System.Collections.Generic;

namespace Swagger.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public List<string> LikeName { get; set; }
        //public List<Comment> Comments { get; set; }
    }
}