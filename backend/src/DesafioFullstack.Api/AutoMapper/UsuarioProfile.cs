using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullstack.Api.Contract.Usuario;
using DesafioFullstack.Api.Domain.Models;

namespace DesafioFullstack.Api.AutoMapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile ()
        {
            CreateMap<Usuario, UsuarioLoginRequestContract>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginResponseContract>().ReverseMap();
            CreateMap<Usuario, UsuarioResponseContract>().ReverseMap();
            CreateMap<Usuario, UsuarioRequestContract>().ReverseMap();
        }
    }
}