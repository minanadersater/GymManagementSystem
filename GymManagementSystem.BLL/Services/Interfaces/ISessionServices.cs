using GymManagementSystem.BLL.Comman;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
         Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
         Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct=default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainaerForDropDownAsync(CancellationToken ct =default );
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct =default );

        Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken ct);
        Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct);

      //  public Task<SessionViewModel?> GetSessionById(int sessionId, CancellationToken ct);
        Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct );
    }
}
