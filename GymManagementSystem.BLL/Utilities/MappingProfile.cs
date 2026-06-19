using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Utilities
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            MapSession();
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>();


            CreateMap<CreateSessionViewModel, Session>()
            .ForMember(
                dest => dest.EndTime,
                opt => opt.MapFrom(src => src.EndDate)
            );
           // CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<Session, UpdateSessionViewModel>()
    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndTime))
    .ReverseMap()
    .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndDate));


        }


        //public MappingProfile()
        //{
        //    CreateMap<Session, SessionViewModel>()
        //        .ForMember(dest => dest.CategoryName,
        //                   opt => opt.MapFrom(src => src.Category.CategoryName))
        //        .ForMember(dest => dest.TrainerName,
        //                   opt => opt.MapFrom(src => src.Trainer.Name))
        //        .ForMember(dest => dest.AvailableSlots,
        //                   opt => opt.Ignore())
        //        .ReverseMap();

        //    CreateMap<CreateSessionViewModel, Session>();

        //    CreateMap<Trainer, TrainerSelectViewModel>();

        //    CreateMap<Category, CategorySelectViewModel>();
        //}
    }
}
