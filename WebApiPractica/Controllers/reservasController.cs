using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listReservas = (from e in _equiposContexto.reservas
                                   join d 
                                   in _equiposContexto.equipos 
                                   on e.equipo_id equals d.id_equipos
                                   join a
                                   in _equiposContexto.usuarios
                                   on e.usuario_id equals a.usuario_id
                                   select new NewRecord(e, d.nombre, d.descripcion, a.nombre, a.carnet)).ToList();

            if (listReservas.Count == 0) return NotFound();

            return Ok(listReservas);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var reserva = (from e in _equiposContexto.reservas
                                   join d 
                                   in _equiposContexto.equipos 
                                   on e.equipo_id equals d.id_equipos
                                   join a
                                   in _equiposContexto.usuarios
                                   on e.usuario_id equals a.usuario_id
                                   where e.reserva_id == id
                                   select new NewRecord(e, d.nombre, d.descripcion, a.nombre, a.carnet)).FirstOrDefault();

            if (reserva == null) return NotFound();

            return Ok(reserva);
        }

        [HttpGet]
        [Route("Find/{Tiempo_reserva}")]
        public IActionResult FindByDescription(int filtro)
        {
            var reserva = (from e in _equiposContexto.reservas
                            join d
                            in _equiposContexto.equipos
                            on e.equipo_id equals d.id_equipos
                            join a
                            in _equiposContexto.usuarios
                            on e.usuario_id equals a.usuario_id
                            where e.tiempo_reserva == filtro
                            select new NewRecord(e, d.nombre, d.descripcion, a.nombre, a.carnet)).FirstOrDefault();

            if (reserva == null) return NotFound();

            return Ok(reserva);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReserva([FromBody] reservas reserva)
        {
            try
            {
                _equiposContexto.reservas.Add(reserva);
                _equiposContexto.SaveChanges();
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] reservas reservaModificar)
        {
            reservas? reservaActual = (from e in _equiposContexto.reservas
                                       where e.reserva_id == id
                                       select e).FirstOrDefault();

            if (reservaActual == null) return NotFound();

            reservaActual.equipo_id = reservaModificar.equipo_id;
            reservaActual.usuario_id = reservaModificar.usuario_id;
            reservaActual.fecha_salida = reservaModificar.fecha_salida;
            reservaActual.hora_salida = reservaModificar.hora_salida;
            reservaActual.tiempo_reserva = reservaModificar.tiempo_reserva;
            reservaActual.estado_reserva_id = reservaModificar.estado_reserva_id;
            reservaActual.fecha_retorno = reservaModificar.fecha_retorno;
            reservaActual.hora_retorno = reservaModificar.hora_retorno;


            _equiposContexto.Entry(reservaActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(reservaModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Eliminarreserva(int id)
        {
            reservas? reserva = (from e in _equiposContexto.reservas
                                 where e.reserva_id == id
                                 select e).FirstOrDefault();

            if (reserva == null)
                return NotFound();

            _equiposContexto.reservas.Attach(reserva);
            _equiposContexto.reservas.Remove(reserva);
            _equiposContexto.SaveChanges();

            return Ok(reserva);
        }
    }

    internal record NewRecord(reservas E, string Nombre, string Descripcion, string Item, string Carnet);
}
