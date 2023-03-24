using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public estados_reservaController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_reserva> listEstado = (from e in _equiposContext.estados_Reservas
                                               select e).ToList();

            if (listEstado.Count == 0) return NotFound();

            return Ok(listEstado);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult Get(int id)
        {
            estados_reserva? estados_ = (from e in _equiposContext.estados_Reservas
                                              where e.estados_res_id == id
                                              select e).FirstOrDefault();

            if (estados_ == null) return NotFound();
            return Ok(estados_);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            estados_reserva? estados_ = (from e in _equiposContext.estados_Reservas
                                        where e.estado.Contains(filtro)
                                        select e).FirstOrDefault();
            if (estados_ == null) return NotFound();
            return Ok(estados_);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstadoR([FromBody] estados_reserva estados_)
        {
            try
            {
                _equiposContext.estados_Reservas.Add(estados_);
                _equiposContext.SaveChanges();
                return Ok(estados_);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEstReserva(int id, [FromBody] estados_reserva estados_Modificar)
        {
            estados_reserva? estados_Actual = (from e in _equiposContext.estados_Reservas
                                              where e.estados_res_id == id
                                              select e).FirstOrDefault();

            if (estados_Actual == null) return NotFound();

            estados_Actual.estado = estados_Modificar.estado;

            return Ok(estados_Actual);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEstadoR(int id)
        {
            estados_reserva? estados_ = (from e in _equiposContext.estados_Reservas
                                        where e.estados_res_id == id
                                        select e).FirstOrDefault();

            if (estados_ == null) return NotFound();

            _equiposContext.estados_Reservas.Attach(estados_);
            _equiposContext.estados_Reservas.Remove(estados_);
            _equiposContext.SaveChanges();

            return Ok(estados_);
        }
    }
}
