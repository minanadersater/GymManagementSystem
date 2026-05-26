using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAll();
        Task<Plan?> GetById(int id);
        void Add(Plan plan);
        void Update(Plan plan);
        void Delete(int id);

        Task<int> CompleteAsync();

    }
}
