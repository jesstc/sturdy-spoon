using System;
using System.Collections.Generic;
using Swagger.Models;

namespace Swagger.Service
{
    public class DictionaryService
    {
        private List<Account> _accountData = new List<Account>();
        private Dictionary<Guid, List<Post>> _postData = new Dictionary<Guid, List<Post>>();
        private Dictionary<Guid, List<Comment>> _commentData = new Dictionary<Guid, List<Comment>>();

        public List<Account> accountData
        {
            get => _accountData;
            set => _accountData = value;
        }

        public Dictionary<Guid, List<Post>> postData
        {
            get => _postData;
            set => _postData = value;
        }

        public Dictionary<Guid, List<Comment>> commentData
        {
            get => _commentData;
            set => _commentData = value;
        }
    }
}