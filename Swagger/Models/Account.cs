using System;
using System.Collections.Generic;

namespace Swagger.Models
{
    public class Account
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Friends { get; set; }
    }
}