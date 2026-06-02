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
        private readonly IGenericRepository<Plan> planRepository;

        public IGenericRepository<Booking> bookingRepository { get; }
        public IGenericRepository<Member> MemberRepository { get; }
        public IGenericRepository<Membership> MembershipRepository { get; }
        public IGenericRepository<HealthRecord> HealthRecordRepository { get; }

        public MemberServices(
           // IGenericRepository<Member> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Member> memberRepository,
            IGenericRepository<Plan> PlanRepository,
            IGenericRepository<HealthRecord> HealthRecordRepository,
            IGenericRepository<Booking> bookingRepository)
        {
            this.MemberRepository = memberRepository;
            this.MembershipRepository = membershipRepository;
            this.planRepository = PlanRepository;
            this.HealthRecordRepository = HealthRecordRepository;
            this.bookingRepository = bookingRepository;
        }


        //get

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await MemberRepository.GetAll(false,ct);
            if (!members.Any()) return [];
            var MembersViewModel = members.Select(m => new MemberViewModel()
            {
                Id=m.Id,
                Name=m.Name,
                Email=m.Email,
                Phone=m.Phone,
                Photo=m.Photo,
                Gender=m.Gender.ToString(),
            });
            return MembersViewModel;
        }

        public async Task<MemberViewModel> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var members = await MemberRepository.GetById(memberId, ct);

            if (members == null) return null;

            var MemberVM = new MemberViewModel()
            {
                Name = members.Name,
                Email = members.Email,
                Phone = members.Phone,
                DateOfBirth = members.DateOfBirth.ToShortDateString(),
                Gender = members.Gender.ToString(),
                Address = $"{members.Address.BuildingNumber} - {members.Address.Street} - {members.Address.City}"

            };

            var ActiveMembership = await MembershipRepository.FirestOrDefaultAsync(mb=>mb.MemberId== memberId &&
            mb.EndDate>DateTime.Now, false, ct);

            if (ActiveMembership is not null)
            {

                var ActivePlan = await planRepository.GetById(ActiveMembership.PlanId, ct);
               
                MemberVM.PlanName = ActivePlan?.Name;
                MemberVM.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
                MemberVM.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
            }
            return MemberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var Record = await HealthRecordRepository.FirestOrDefaultAsync(x => x.MemberId == memberId, false, ct);
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

            var member = await MemberRepository.GetById(memberId, ct);
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
            var emailExist =  await MemberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExist = await MemberRepository.AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExist || phoneExist) return false;
            var Member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = (DAL.Entities.Gender)model.Gender,
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

            MemberRepository.Add(Member);
            var Result = await MemberRepository.CompleteAsync();
            return Result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await MemberRepository.GetById(id, ct);
            if (member is null) return false;

            if (await MemberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id )) return false;
            if (await MemberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != id)) return false;
           
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.BuildingNumber  = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.UpdatedAt =DateTime.Now;
            var Result = await MemberRepository.CompleteAsync();    
            return Result >= 0 ? true : false;
        }


        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {             
           var HasfutureSession  = await bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.EndTime > DateTime.Now);
            if (HasfutureSession) return false;
            MemberRepository.Delete(memberId);
            var Result = await MemberRepository.CompleteAsync();
            return Result > 0;

        }

      
    }
}
