using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DAL.EF;

namespace Services.Services
{
    public abstract class BaseService
    {
        protected readonly MyDbContext _dbContext;
        protected readonly IMapper _mapper;
        public BaseService(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
