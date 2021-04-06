using System;
using System.Collections.Generic;

namespace Swagger.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public List<string> LikeName { get; set; }
    }
}