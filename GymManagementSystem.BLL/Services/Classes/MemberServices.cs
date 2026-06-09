using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembersViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Entities.Enums;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;

        //private readonly IGenericRepository<Plan> planRepository;
        //public readonly IGenericRepository<Booking> bookingRepository;
        //public readonly IGenericRepository<Member> MemberRepository;
        //public readonly IGenericRepository<Membership> MembershipRepository;
        //public readonly IGenericRepository<HealthRecord> HealthRecordRepository;

        //public MemberServices(
        //   // IGenericRepository<Member> memberRepository,
        //    IGenericRepository<Membership> membershipRepository,
        //    IGenericRepository<Member> memberRepository,
        //    IGenericRepository<Plan> PlanRepository,
        //    IGenericRepository<HealthRecord> HealthRecordRepository,
        //    IGenericRepository<Booking> bookingRepository)
        //{
        //    this.MemberRepository = memberRepository;
        //    this.MembershipRepository = membershipRepository;
        //    this.planRepository = PlanRepository;
        //    this.HealthRecordRepository = HealthRecordRepository;
        //    this.bookingRepository = bookingRepository;
        //}

        public MemberServices(IUnitOfWork unitOfWork) 
        {
            this.unitOfWork = unitOfWork;
        }
        //get

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false,ct);
            if (!members.Any()) return [];
            var MembersViewModel = members.Select(m => new MemberViewModel()
            {
                Id=m.Id,
                Name=m.Name,
                Email=m.Email,
                Phone=m.Phone,
                Photo=m.Photo,
                //Gender=m.Gender.ToString(),
                Gender=m.Gender,

            });
            return MembersViewModel;
        }

        public async Task<MemberViewModel> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);

            if (members == null) return null;

            var MemberVM = new MemberViewModel()
            {
                Name = members.Name,
                Email = members.Email,
                Phone = members.Phone,
                DateOfBirth = members.DateOfBirth.ToShortDateString(),
                Gender =members.Gender,
                Address = $"{members.Address.BuildingNumber} - {members.Address.Street} - {members.Address.City}"

            };

            var ActiveMembership = await unitOfWork.GetRepository<Membership>().FirestOrDefaultAsync(mb=>mb.MemberId== memberId &&
            mb.EndDate>DateTime.Now, false, ct);

            if (ActiveMembership is not null)
            {

                var ActivePlan = await unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId, ct);
               
                MemberVM.PlanName = ActivePlan?.Name;
                MemberVM.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
                MemberVM.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
            }
            return MemberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await unitOfWork.GetRepository<HealthRecord>().FirestOrDefaultAsync(x => x.MemberId == memberId, false, ct);
            if (Record == null) return null;
            
            else
            return new HealthRecordViewModel()
            {
                
                Height = Record.Height,
                Weight = Record.Weight,
                BloodType = Record.BloodType,
                Note = Record.Note,

            };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateRecordAsync(int memberId, CancellationToken ct = default)
        {

            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member == null) return null;
            else

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Photo = member.Photo,

            };

        }

        //post
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExist =  await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExist || phoneExist) return false;
            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                // 
                Gender = model.Gender.Value,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note,
                }             
            };

            unitOfWork.GetRepository<Member>().Add(Member);
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member is null) return false;

            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id )) return false;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id)) return false;
           
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.BuildingNumber  = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.UpdatedAt =DateTime.Now;
            var Result = await unitOfWork.CompleteAsync();    
            return Result >= 0 ? true : false;
        }


        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {             
           var HasfutureSession  = await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.EndTime > DateTime.Now);
            if (HasfutureSession) return false;
            unitOfWork.GetRepository<Member>().Delete(memberId);
            var Result = await unitOfWork.CompleteAsync();
            return Result > 0;

        }

      
    }
}
