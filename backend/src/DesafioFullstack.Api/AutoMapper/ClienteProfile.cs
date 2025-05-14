using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Cliente;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.AutoMapper
{
        public class ClienteProfile : Profile
         {
            public ClienteProfile()
            {
                CreateMap<Cliente, ClienteResponseContract>().ReverseMap();
                CreateMap<Cliente, ClienteRequestContract>().ReverseMap();
             }
        
    }
    
}