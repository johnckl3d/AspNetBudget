using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<Meetup, MeetupDetailsDto>()
                .ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
                .ForMember(m => m.PostCode, map => map.MapFrom(meetup => meetup.Location.PostCode))
                .ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street));

            CreateMap<MeetupDto, Meetup>();

            CreateMap<Budget, BudgetDetailsDto>();

            CreateMap<CostItemDto, CostItem>().ReverseMap();

            CreateMap<LectureDto, Lecture>()
                .ReverseMap();
        }
    }
}
