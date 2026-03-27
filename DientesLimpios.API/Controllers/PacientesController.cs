using DientesLimpios.API.DTOs.Pacientes;
using DientesLimpios.API.Utilidades;
using DientesLimpios.Aplicacion.CasosDeUso.Pacientes.Comandos.ActualizarPaciente;
using DientesLimpios.Aplicacion.CasosDeUso.Pacientes.Comandos.BorrarPaciente;
using DientesLimpios.Aplicacion.CasosDeUso.Pacientes.Comandos.CrearPaciente;
using DientesLimpios.Aplicacion.CasosDeUso.Pacientes.Consultas.ObtenerDetallePaciente;
using DientesLimpios.Aplicacion.CasosDeUso.Pacientes.Consultas.ObtenerListadoPacientes;
using DientesLimpios.Aplicacion.Utilidades.Comunes;
using DientesLimpios.Aplicacion.Utilidades.Mediador;
using Microsoft.AspNetCore.Mvc;

namespace DientesLimpios.API.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PacienteListadoDTO>>> Get([FromQuery] ConsultaObtenerListadoPacientes consulta)
        {
            var resultado = await _mediator.Send(consulta);
            HttpContext.InsertarPaginacionEnCabecera(resultado.Total);

            return resultado.Elementos;
        }

        [HttpGet("paginacion2")]
        public async Task<ActionResult<PaginadoDTO<PacienteListadoDTO>>> Get2([FromQuery] ConsultaObtenerListadoPacientes consulta)
        {
            var resultado = await _mediator.Send(consulta);

            return resultado;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDetalleDTO>> Get(Guid id)
        {
            var consulta = new ConsultaObtenerDetallePaciente { Id = id };
            var resultado = await _mediator.Send(consulta);

            return resultado;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CrearPacienteDTO crearPacienteDTO)
        {
            var comando = new ComandoCrearPaciente { Nombre = crearPacienteDTO.Nombre, Email = crearPacienteDTO.Email };
            await _mediator.Send(comando);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ActualizarPacienteDTO actualizarPacienteDTO)
        {
            var comando = new ComandoActualizarPaciente { Id = id, Nombre = actualizarPacienteDTO.Nombre, Email = actualizarPacienteDTO.Email };
            await _mediator.Send(comando);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comando = new ComandoBorrarPaciente { Id = id };
            await _mediator.Send(comando);

            return Ok();
        }
    }
}
