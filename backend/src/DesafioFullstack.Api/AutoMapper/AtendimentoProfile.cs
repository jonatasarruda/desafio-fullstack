using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Atendimentos;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.AutoMapper
{
    public class AtendimentoProfile : Profile
    {
        public AtendimentoProfile()
        {
              
            CreateMap<Atendimento, AtendimentoResponseContract>().ReverseMap();
            CreateMap<Atendimento, AtendimentoRequestContract>().ReverseMap();
        }
        
    }
}