using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using System.Data;

namespace Mafiator.Repository.Repositories
{
    public class EventRepository:Repository<Event>,IEventRepository
    {
        public EventRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }
    }
}
