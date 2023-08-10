using AutoMapper;
using Order.Domain.Model.ResponseModel;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderRequestModel, Orders>().ReverseMap();
            CreateMap<Orders, OrderResponseModel>().ReverseMap();
        }
    }
}
