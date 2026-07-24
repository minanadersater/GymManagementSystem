using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;

            var upcomingSessions = await unitOfWork.GetRepository<Session>()
                .CountAsync(s => s.StartDate > now, ct);

            var ongoingSessions = await unitOfWork.GetRepository<Session>()
                .CountAsync(s => s.StartDate <= now && s.EndTime >= now, ct);

            var completedSessions = await unitOfWork.GetRepository<Session>()
                .CountAsync(s => s.EndTime < now, ct);

            var totalMembers = await unitOfWork.GetRepository<Member>()
                .CountAsync(ct: ct);

            var totalTrainers = await unitOfWork.GetRepository<Trainer>()
                .CountAsync(ct: ct);

            var activeMembers = await unitOfWork.GetRepository<Membership>()
                .CountAsync(m => m.EndDate > now, ct);

            return new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }
    }
}