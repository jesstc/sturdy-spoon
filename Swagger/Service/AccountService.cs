using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models;

namespace Swagger.Service
{
    public class AccountService
    {
        private readonly DictionaryService _dbdata;
        private List<Account> account = new List<Account>();

        public Account CreateAccount(string Name, int Age)
        {
            var newAccount = new Account();
            newAccount.UserId = Guid.NewGuid();
            newAccount.Name = Name;
            newAccount.Age = Age;

            return newAccount;
        }

    }
}