using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices MemberServices;

        public MemberController(IMemberServices MemberServices)
        {
            this.MemberServices = MemberServices;
        }
       public async Task<IActionResult> Index(CancellationToken  ct)
        {
            var Result = TempData["Result"];
            var Members = await MemberServices.GetAllMembersAsync(ct);
            return View(Members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View("Create", model);
            
            var Result = await MemberServices.CreateMemberAsync(model, ct);
            if(Result)
                 TempData["SuccessMessage"] = "Member created successfully.";
            else
                 TempData["ErrorMessage"] = "Failed to create member.";

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var Member = await MemberServices.GetMemberDetailsAsync(id, ct);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }
            return View(Member);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var HealthRecord = await MemberServices.GetMemberHealthRecordDetailsAsync(id, ct);
            if (HealthRecord == null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction("Index");
            }
            return View(HealthRecord);
        }
    }
}
