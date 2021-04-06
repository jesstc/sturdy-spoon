using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swagger.Models;
using Swagger.Service;

namespace Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DictionaryService _dbdata;

        public AccountController(DictionaryService dictionaryService)
        {
            _dbdata = dictionaryService;
        }

        [HttpGet("GetAllAccountData")]
        public ActionResult<List<Account>> GetAllAccount()
        {
            return Ok(_dbdata.accountData);
        }

        [HttpPost("CreateAccount")]
        public ActionResult<Account> CreateAccount(string Name, int Age)
        {
            var newAccount = new Account() { UserId = Guid.NewGuid(), Name = Name, Age = Age, Friends = null };
            _dbdata.accountData.Add(newAccount);

            return Ok(newAccount);
        }

        [HttpPut("Update")]
        public ActionResult<Account> UpdateAccount(Guid updateId, string newName, int newAge)
        {
            _dbdata.accountData.Where(x => x.UserId.Equals(updateId)).ToList().ForEach(x =>
                {
                    x.Name = newName;
                    x.Age = newAge;
                });

            return Ok(_dbdata.accountData);
        }

        [HttpDelete("Delete")]
        public ActionResult<Account> DeleteAccount(Guid userId, string Name)
        {
            var changedRaw = _dbdata.accountData.RemoveAll(x => x.UserId.Equals(userId) || x.Name.Equals(Name));
            if (changedRaw == 0) return Ok("Cannot find match data");
            else return Ok(_dbdata.accountData);
        }
    }
}
