using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
//﻿using Castle.Core.Configuration;

namespace TestProject1
{
        public class DatabaseFixture : IDisposable
        {
            public db_shopContext Context { get; private set; }

            public DatabaseFixture()
            {

                // Set up the test database connection and initialize the context
                var options = new DbContextOptionsBuilder<db_shopContext>()

                    .UseSqlServer("Data Source=DESKTOP-682T053;Initial Catalog=215806571_shop;Integrated Security=True;Trust Server Certificate=True")
                    .Options;
                Context = new db_shopContext(options);
                Context.Database.EnsureCreated();
            }

            public void Dispose()
            {
                // Clean up the test database after all tests are completed
                Context.Database.EnsureDeleted();
                Context.Dispose();
            }
        }
    
}
