using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;

namespace Services.Services
{
    public abstract class BaseService
    {
        protected readonly MyDbContext _dbContext;
        public BaseService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
