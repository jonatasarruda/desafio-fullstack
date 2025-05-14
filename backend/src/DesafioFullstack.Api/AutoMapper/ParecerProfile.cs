using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Pareceres;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.AutoMapper
{
    public class ParecerProfile : Profile
    {
        public ParecerProfile()
        {
              
            CreateMap<Parecer, ParecerResponseContract>().ReverseMap();
            CreateMap<Parecer, ParecerRequestContract>().ReverseMap();
        }
        
    }
}
