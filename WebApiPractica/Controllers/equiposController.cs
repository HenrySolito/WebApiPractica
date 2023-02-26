using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiPractica.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public equiposController(equiposContext equiposContexto) 
        {
            _equiposContext = equiposContexto;

        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<equipos> listadoEquipo = (from e in _equiposContext.equipos
                                           select e).ToList();

            if (listadoEquipo.Count == 0){
                return NotFound();
            }

            return Ok(listadoEquipo);
        }
    }
}
