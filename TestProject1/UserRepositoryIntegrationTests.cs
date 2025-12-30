using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class UserRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly db_shopContext _dbContext;
        private readonly UserRepository _userRepository;
        public UserRepositoryIntegrationTests(DatabaseFixture databaseFixture) { 
            _dbContext = databaseFixture.Context;
            _userRepository = new UserRepository(_dbContext);
        }

    }
}
