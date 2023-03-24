using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosControllers : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public usuariosControllers(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoCarreras = (from e in _equiposContexto.usuarios
                                   join d in _equiposContexto.carreras on 
                                   e.carrera_id equals d.carrera_id
                                   select new { e, d.nombre_carrera, d.facultad_id }).ToList();
            if (listadoCarreras.Count == 0) return NotFound();

            return Ok(listadoCarreras);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var usuario = (from e in _equiposContexto.usuarios
                            join d in _equiposContexto.carreras on
                            e.carrera_id equals d.carrera_id
                            where e.usuario_id == id
                            select new { e, d.nombre_carrera, d.facultad_id }).FirstOrDefault();

            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        [Route("Finf/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            var usuario = (from e in _equiposContexto.usuarios
                            join d
                           in _equiposContexto.carreras
                           on e.carrera_id equals d.carrera_id
                            where e.nombre.Contains(filtro)
                            select new { e, d.nombre_carrera, d.facultad_id }).FirstOrDefault();

            if (usuario == null) return NotFound();

            return Ok(usuario);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _equiposContexto.usuarios.Add(usuario);
                _equiposContexto.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _equiposContexto.usuarios
                                       where e.usuario_id == id
                                       select e).FirstOrDefault();

            if (usuarioActual == null) return NotFound();

            usuarioActual.carrera_id = usuarioModificar.carrera_id;
            usuarioActual.usuario_id = usuarioModificar.usuario_id;
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.documento = usuarioModificar.documento;
            usuarioActual.tipo = usuarioModificar.tipo;
            usuarioActual.carnet = usuarioModificar.carnet;

            _equiposContexto.Entry(usuarioActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(usuarioModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            usuarios? usuario = (from e in _equiposContexto.usuarios
                                 where e.usuario_id == id
                                 select e).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            _equiposContexto.usuarios.Attach(usuario);
            _equiposContexto.usuarios.Remove(usuario);
            _equiposContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}
