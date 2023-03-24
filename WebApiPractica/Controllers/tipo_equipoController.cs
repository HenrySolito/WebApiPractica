using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listTequipo = (from e in _equiposContext.tipo_equipo
                                           select e).ToList();

            if (listTequipo.Count == 0) return NotFound();

            return Ok(listTequipo);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult Get(int id)
        {
            tipo_equipo? tipo_ = (from e in _equiposContext.tipo_equipo
                                    where e.id_tipo_equipo == id
                                    select e).FirstOrDefault();

            if (tipo_ == null) return NotFound();
            return Ok(tipo_);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            tipo_equipo? Tequipo_ = (from e in _equiposContext.tipo_equipo
                                    where e.descripcion.Contains(filtro)
                                    select e).FirstOrDefault();
            if (Tequipo_ == null) return NotFound();
            return Ok(Tequipo_);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarTequip([FromBody] tipo_equipo tEquipo_)
        {
            try
            {
                _equiposContext.tipo_equipo.Add(tEquipo_);
                _equiposContext.SaveChanges();
                return Ok(tEquipo_);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarTequip(int id, [FromBody] tipo_equipo tEquipo_)
        {
            tipo_equipo? tE_ = (from e in _equiposContext.tipo_equipo
                                    where e.id_tipo_equipo == id
                                    select e).FirstOrDefault();

            if (tE_ == null) return NotFound();

            tE_.descripcion = tEquipo_.descripcion;
            tE_.estado = tEquipo_.estado;

            return Ok(tE_);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarTequip(int id)
        {
            tipo_equipo? tEquip_ = (from e in _equiposContext.tipo_equipo
                                where e.id_tipo_equipo == id
                                select e).FirstOrDefault();

            if (tEquip_ == null) return NotFound();

            _equiposContext.tipo_equipo.Attach(tEquip_);
            _equiposContext.tipo_equipo.Remove(tEquip_);
            _equiposContext.SaveChanges();

            return Ok(tEquip_);
        }
    }
}
