using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullstack.Api.Controllers
{
    public class BaseController: ControllerBase
    {
         protected long _idUsuario; 
        protected long ObterIdUsuarioLogado()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            long.TryParse(id, out long idUsuario);

            return idUsuario;
        }
    }
}