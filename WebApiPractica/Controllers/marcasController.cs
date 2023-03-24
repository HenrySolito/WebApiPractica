using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPractica.Models;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public marcasController(equiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listmarcas = (from e in _equiposContext.marcas
                                               select e).ToList();

            if (listmarcas.Count == 0) return NotFound();

            return Ok(listmarcas);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult Get(int id)
        {
            marcas? marca = (from e in _equiposContext.marcas
                                              where e.id_marcas == id
                                              select e).FirstOrDefault();

            if (marca == null) return NotFound();
            return Ok(marca);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            marcas? marca = (from e in _equiposContext.marcas
                                        where e.nombre_marca.Contains(filtro)
                                        select e).FirstOrDefault();
            if (marca == null) return NotFound();
            return Ok(marca);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarcas([FromBody] marcas marcas_)
        {
            try
            {
                _equiposContext.marcas.Add(marcas_);
                _equiposContext.SaveChanges();
                return Ok(marcas_);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarMarcas(int id, [FromBody] marcas marcas_Modificar)
        {
            marcas? marcas_actual = (from e in _equiposContext.marcas
                                              where e.id_marcas == id
                                              select e).FirstOrDefault();

            if (marcas_actual == null) return NotFound();

            marcas_actual.nombre_marca = marcas_Modificar.nombre_marca;
            marcas_actual.estados = marcas_Modificar.estados;

            return Ok(marcas_actual);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarMarcas(int id)
        {
            marcas? marca = (from e in _equiposContext.marcas
                                        where e.id_marcas == id
                                        select e).FirstOrDefault();

            if (marca == null) return NotFound();

            _equiposContext.marcas.Attach(marca);
            _equiposContext.marcas.Remove(marca);
            _equiposContext.SaveChanges();

            return Ok(marca);
        }
    }
}
