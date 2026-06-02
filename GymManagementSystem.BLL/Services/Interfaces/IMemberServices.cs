using GymManagementSystem.BLL.ViewModels.MembersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public  interface IMemberServices
    {
        //Get model ->ViewModel -> view
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync( CancellationToken ct = default);
        Task<MemberViewModel> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);

        Task<HealthRecordViewModel?> GetMemberHealthRecordDetailsAsync(int memberId, CancellationToken ct = default);
        //Task<HealthRecordViewModel?> GetMemberToUpdateRecordAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateRecordAsync(int memberId, CancellationToken ct = default);
        //Post model ->ViewModel -> DB
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
    
        Task<bool> UpdateMemberDetailsAsync(int Id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int Id, CancellationToken ct = default);

 
    }
}
