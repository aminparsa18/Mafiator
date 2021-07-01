using System.Data;
using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;

namespace Mafiator.Repository.Repositories
{
   public class EventJoinRepository:Repository<EventJoin>,IEventJoinRepository
    {
        public EventJoinRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }

    }
}
