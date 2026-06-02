using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class MemberRepository :GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDbcontext dbContext;
        public MemberRepository(GymDbcontext dbContext): base(dbContext) 
        {
           this.dbContext = dbContext;
        }

    }
}
