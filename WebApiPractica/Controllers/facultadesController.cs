using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listEstado = (from e in _equiposContext.facultades
                                               select e).ToList();

            if (listEstado.Count == 0) return NotFound();

            return Ok(listEstado);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult Get(int id)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                                              where e.facultad_id == id
                                              select e).FirstOrDefault();

            if (facultad == null) return NotFound();
            return Ok(facultad);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                                        where e.nombre_facultad.Contains(filtro)
                                        select e).FirstOrDefault();
            if (facultad == null) return NotFound();
            return Ok(facultad);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarFacultad([FromBody] facultades facultades_)
        {
            try
            {
                _equiposContext.facultades.Add(facultades_);
                _equiposContext.SaveChanges();
                return Ok(facultades_);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarFacultades(int id, [FromBody] facultades facultades)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                                              where e.facultad_id == id
                                              select e).FirstOrDefault();

            if (facultad == null) return NotFound();

            facultad.nombre_facultad = facultades.nombre_facultad;

            return Ok(facultad);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarFacu(int id)
        {
            facultades? facu = (from e in _equiposContext.facultades
                                        where e.facultad_id == id
                                        select e).FirstOrDefault();

            if (facu == null) return NotFound();

            _equiposContext.facultades.Attach(facu);
            _equiposContext.facultades.Remove(facu);
            _equiposContext.SaveChanges();

            return Ok(facu);
        }
    }
}
