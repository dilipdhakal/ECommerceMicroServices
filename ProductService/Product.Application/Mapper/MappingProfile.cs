using AutoMapper;
using Prodcut.Domain.Entities;
using Product.Domain.Model.RequestModel;
using Product.Domain.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductRequestModel, Products>().ReverseMap();
            CreateMap<Products, ProductResponseModel>().ReverseMap();
        }
    }
}
