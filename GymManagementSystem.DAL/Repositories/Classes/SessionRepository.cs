using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbcontext dbContext;

        public SessionRepository(GymDbcontext dbContext): base(dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>>GetAllSessionsByTrainerAndCategoryAsync(CancellationToken ct)
        {
           // var Sessions = dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            var Sessions = dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);
            return await Sessions.ToListAsync(ct);
        }

      

        public Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct)
        {
            return dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);
            // return dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }

        public async Task<Session> GetSessionsByIdTrainerAndCategoryAsync(int sessionId,CancellationToken ct)
        {
            var Session = dbContext.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == sessionId, ct);
            return await Session;
        }

       
    }
}
