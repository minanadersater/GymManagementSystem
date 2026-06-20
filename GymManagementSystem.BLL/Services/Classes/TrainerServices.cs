using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerServices(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>>
            GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork
                .GetRepository<Trainer>()
                .GetAll(false, ct);

            return mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?>
            GetTrainerDetailsAsync(
            int trainerId,
            CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetById(trainerId, ct);

            if (trainer is null)
                return null;

            return mapper.Map<TrainerViewModel>(trainer);
        }

        public async Task<bool>
            CreateTrainerAsync(
            CreateTrainerViewModel model,
            CancellationToken ct = default)
        {
            var emailExist = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(t => t.Email == model.Email, ct);

            var phoneExist = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(t => t.Phone == model.Phone, ct);

            if (emailExist || phoneExist)
                return false;

            var trainer = mapper.Map<Trainer>(model);

            unitOfWork
                .GetRepository<Trainer>()
                .Add(trainer);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<TrainerToUpdateViewModel?>
            GetTrainerToUpdateAsync(
            int trainerId,
            CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetById(trainerId, ct);

            if (trainer is null)
                return null;

            return mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public async Task<bool>
            UpdateTrainerAsync(
            int id,
            TrainerToUpdateViewModel model,
            CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetById(id, ct);

            if (trainer is null)
                return false;

            var emailExist = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(t => t.Email == model.Email && t.Id != id, ct);

            var phoneExist = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(t => t.Phone == model.Phone && t.Id != id, ct);

            if (emailExist || phoneExist)
                return false;

            mapper.Map(model, trainer);

            trainer.UpdatedAt = DateTime.Now;

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool>
            DeleteTrainerAsync(
            int trainerId,
            CancellationToken ct = default)
        {
            var hasFutureSessions = await unitOfWork
                .GetRepository<Session>()
                .AnyAsync(
                    s => s.TrainerId == trainerId &&
                         s.EndTime > DateTime.Now,
                    ct);

            if (hasFutureSessions)
                return false;

            unitOfWork
                .GetRepository<Trainer>()
                .Delete(trainerId);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}
