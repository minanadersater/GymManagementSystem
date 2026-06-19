using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsByTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetSessionsByIdTrainerAndCategoryAsync(int sessionId,CancellationToken ct);
   
        Task<int>GetCountOfBookedSlotAsync(int sessionId,CancellationToken ct);

    
    }
}
