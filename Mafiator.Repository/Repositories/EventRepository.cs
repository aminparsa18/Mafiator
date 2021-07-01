using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;

namespace Mafiator.Repository.Repositories
{
    public class EventRepository:Repository<Event>,IEventRepository
    {
        public EventRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }
    }
}
