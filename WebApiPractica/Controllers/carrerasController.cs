using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApiPractica.Properties;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoCarreras = (from e in _equiposContexto.carreras join d in 
                                              _equiposContexto.facultades on
                                              e.facultad_id equals d.facultad_id
                                              select new {e, d.nombre_facultad}).ToList();
            if(listadoCarreras.Count == 0) return NotFound();

            return Ok(listadoCarreras);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var carreras = (from e in _equiposContexto.carreras join d 
                                  in _equiposContexto.facultades 
                                  on e.facultad_id 
                                  equals d.facultad_id 
                                  where e.carrera_id == id 
                                  select  new {e, d.facultad_id,d.nombre_facultad}).FirstOrDefault();
            
            if(carreras == null) return NotFound();
            
            return Ok(carreras);
        }

        [HttpGet]
        [Route("Finf/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            var carreras =(from e in _equiposContexto.carreras join d 
                           in _equiposContexto.facultades
                           on e.facultad_id equals d.facultad_id
                           where e.nombre_carrera.Contains(filtro)
                           select new {e, d.nombre_facultad }).FirstOrDefault();
            
            if (carreras == null) return NotFound();

            return Ok(carreras);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCarrera([FromBody] carreras carreras)
        {
            try
            {
                _equiposContexto.carreras.Add(carreras);
                _equiposContexto.SaveChanges();
                return Ok(carreras);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCarrera(int id, [FromBody] carreras carreraModificar)
        {
            carreras? carreraActual = (from e in _equiposContexto.carreras
                                       where e.carrera_id == id
                                       select e).FirstOrDefault();

            if(carreraActual == null) return NotFound();

            carreraActual.carrera_id = carreraModificar.carrera_id;
            carreraActual.nombre_carrera = carreraModificar.nombre_carrera;
            carreraActual.facultad_id = carreraModificar.facultad_id;
            
            _equiposContexto.Entry(carreraActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(carreraModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCarrera(int id) 
        {
            carreras? carrera = (from e in _equiposContexto.carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();

            if (carrera == null)
                return NotFound();

            _equiposContexto.carreras.Attach(carrera);
            _equiposContexto.carreras.Remove(carrera);
            _equiposContexto.SaveChanges();

            return Ok(carrera);
        }
    }
}
