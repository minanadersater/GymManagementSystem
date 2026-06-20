using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapTrainer();
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

        private void MapTrainer()
        {
           // CreateMap<Trainer, TrainerViewModel>();
            CreateMap<Trainer, TrainerViewModel>()
    .ForMember(dest => dest.Address,
        opt => opt.MapFrom(src =>
            src.Address == null
                ? ""
                : $"{src.Address.BuildingNumber}, {src.Address.Street}, {src.Address.City}"
        )).ForMember(dest => dest.Specialties,
        opt => opt.MapFrom(src => src.Specialize));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Specialize,
                    opt => opt.MapFrom(src => src.Specialties))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => new Address
                    {
                        BuildingNumber = src.BuildingNumber,
                        City = src.City,
                        Street = src.Street
                    }));
            //CreateMap<TrainerToUpdateViewModel, Trainer>().ReverseMap();
            //  CreateMap<Trainer, TrainerDetailsViewModel>();


            CreateMap<TrainerToUpdateViewModel, Trainer>()
    .ForMember(dest => dest.Address,
        opt => opt.MapFrom(src => new Address
        {
            BuildingNumber = src.BuildingNumber,
            City = src.City,
            Street = src.Street
        }))
       .ForMember(dest => dest.Specialize,
        opt => opt.MapFrom(src => src.Specialties));
            

            CreateMap<Trainer, TrainerToUpdateViewModel>()
    .ForMember(dest => dest.BuildingNumber,
        opt => opt.MapFrom(src => src.Address.BuildingNumber))
    .ForMember(dest => dest.City,
        opt => opt.MapFrom(src => src.Address.City))
    .ForMember(dest => dest.Street,
        opt => opt.MapFrom(src => src.Address.Street));
        }


    }
}
