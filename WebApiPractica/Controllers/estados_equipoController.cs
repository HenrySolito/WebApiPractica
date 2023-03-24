using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_equipo> listEstado = (from e in _equiposContext.estados_equipo
                                               select e).ToList();

            if (listEstado.Count == 0) return NotFound();

            return Ok(listEstado);
        }
        
        [HttpGet]
        [Route("GetById")]
        public IActionResult Get(int id)
        {
            estados_equipo? estados_Equipo = (from e in _equiposContext.estados_equipo
                                              where e.id_estados_equipo == id
                                              select e).FirstOrDefault();

            if(estados_Equipo == null) return NotFound();
            return Ok(estados_Equipo);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro) 
        {
            estados_equipo? estados_ = (from e in _equiposContext.estados_equipo
                                        where e.descripcion.Contains(filtro)
                                        select e).FirstOrDefault();
            if(estados_ == null) return NotFound();
            return Ok(estados_);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstadoE([FromBody] estados_equipo estados_)
        {
            try
            {
                _equiposContext.estados_equipo.Add(estados_);
                _equiposContext.SaveChanges();
                return Ok(estados_);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEstEquipo(int id, [FromBody] estados_equipo estados_Modificar)
        {
            estados_equipo? estados_Actual = (from e in _equiposContext.estados_equipo
                                              where e.id_estados_equipo == id
                                              select e).FirstOrDefault();

            if (estados_Actual == null) return NotFound();

            estados_Actual.estado = estados_Modificar.estado;
            estados_Actual.descripcion = estados_Modificar.descripcion;

            return Ok(estados_Actual);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEstado(int id)
        {
            estados_equipo? estados_ = (from e in _equiposContext.estados_equipo
                                        where e.id_estados_equipo == id
                                        select e).FirstOrDefault();

            if(estados_ == null) return NotFound();

            _equiposContext.estados_equipo.Attach(estados_);
            _equiposContext.estados_equipo.Remove(estados_);
            _equiposContext.SaveChanges();

            return Ok(estados_);
        }
    }
}
