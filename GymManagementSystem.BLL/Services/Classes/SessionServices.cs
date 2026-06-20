
using AutoMapper;
using GymManagementSystem.BLL.Comman;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices 
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionServices(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be Greater Than Start Date");
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId,ct);
            if (Trainer is null)
                return Result.NotFound("Trainer not found");
            var CategoryRepo = unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetById(model.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category not found");

            var Session = mapper.Map<CreateSessionViewModel, Session>(model);


            var SessionRepo = unitOfWork.GetRepository<Session>();

            SessionRepo.Add(Session);
            var rowEffected = await unitOfWork.CompleteAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Faild("Failed to create session");


        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct)
        {
           var repo = unitOfWork.GetRepository<Session>();

            var session = await repo.GetById(sessionId, ct);
            if(session is null)
                return Result.NotFound("Session not found");

            if(session.EndTime >= DateTime.Now)
                return Result.Faild("Can not delete session that has already ended.");

            var BookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(sessionId, ct);
            if (BookedCount > 0)
                return Result.Faild("Can not delete session that has booked slots.");

            repo.Delete(sessionId);
            var EffectedRows = await unitOfWork.CompleteAsync();
            return EffectedRows > 0 ? Result.Ok() : Result.Faild("Failed to delete session");

        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
           // var Sessions = await unitOfWork.GetRepository<Session>().GetAll();
           var Sessions =await unitOfWork.SessionRepository.GetAllSessionsByTrainerAndCategoryAsync(ct);
            if (!Sessions.Any()) return null;

            Sessions = Sessions.OrderByDescending(x => x.StartDate);
            //var MappedSessions = Sessions.Select(x => new SessionViewModel()
            //{
            //    //Id = x.Id,
            //    //Description = x.Description,
            //    //Capacity = x.Capacity,
            //    //StartDate = x.StartTime,
            //    //EndDate = x.EndTime,
            //    //TrainerName = x.Trainer.FullName,
            //    //CategoryName = x.Category.Name,
            //    //AvailableSlots = x.Capacity - x.Members.Count
            //}
            
            var MappedSessions = mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);
           // var MappedSessions = mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);

                       
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
               
            }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {

            var categorios = await unitOfWork.GetRepository<Category>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categorios);
        }

        public async Task<SessionViewModel?> GetSessionById(int sessionId, CancellationToken ct)
        {
           var session = await unitOfWork.GetRepository<Session>().GetById(sessionId);
          return mapper.Map<Session, SessionViewModel>(session);
        }

        public async Task<SessionViewModel> GetSessionByIdAsync(int sessionId, CancellationToken ct)
        {
            var Session = await unitOfWork.SessionRepository.GetSessionsByIdTrainerAndCategoryAsync(sessionId, ct);
            if (Session is null)
                return null;
            var mappedSession = mapper.Map<Session, SessionViewModel>(Session);
            mappedSession.AvailableSlots = mappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(sessionId, ct);
            return mappedSession;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
          var Session =await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
          if (Session is null)
              return null;

            if (!await IsSessionValidForUpdateAsync(Session, ct))
                return null;
            return mapper.Map<Session, UpdateSessionViewModel>(Session);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainaerForDropDownAsync(CancellationToken ct = default)
        {
            var Trainers =await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);

            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);

        }

        public async Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct)
        {
            var SessionRepo = unitOfWork.GetRepository<Session>();
            var session = await SessionRepo.GetById(sessionId, ct);

            if (session is null)
                return Result.NotFound("Session not found");
            if(session.StartDate <= DateTime.Now)
                return Result.Faild("Can not update session that has already started.");
            var Booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
           if(Booked>0 )
                return Result.Faild("Can not update session that has booked slots.");
           if(model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date.");
            if ((model.StartDate<= DateTime.Now))
                return Result.Validation("Start date must be in the future.");

            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);
            if (Trainer is null)
                return Result.NotFound("Trainer not found");

          session.UpdatedAt=DateTime.Now;
            mapper.Map(model, session);
            SessionRepo.Update(session);
            var EffectedRows = await unitOfWork.CompleteAsync();

            return EffectedRows > 0 ? Result.Ok() : Result.Faild("Failed to update session");

        }

        private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var Booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            return Booked == 0;
        }



    }
}
