using DientesLimpios.Aplicacion.Contratos.Persistencia;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Aplicacion.Excepciones;
using DientesLimpios.Aplicacion.Utilidades.Mediador;

namespace DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.ActualizarConsultorio
{
    public class CasoDeUsoActualizarConsultorio : IRequestHandler<ComandoActualizarConsultorio>
    {
        private readonly IRepositorioConsultorios _repositorio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CasoDeUsoActualizarConsultorio(IRepositorioConsultorios repositorio, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorio = repositorio;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public async Task Handle(ComandoActualizarConsultorio request)
        {
            var consultorio = await _repositorio.ObtenerPorId(request.Id);

            if (consultorio is null)
            {
                throw new ExcepcionNoEncontrado();
            }

            consultorio.ActualizarNombre(request.Nombre);

            try
            {
                await _repositorio.Actualizar(consultorio);
                await _unidadDeTrabajo.Persistir();
            }
            catch (Exception)
            {
                await _unidadDeTrabajo.Revertir();
                throw;
            }
        }
    }
}
