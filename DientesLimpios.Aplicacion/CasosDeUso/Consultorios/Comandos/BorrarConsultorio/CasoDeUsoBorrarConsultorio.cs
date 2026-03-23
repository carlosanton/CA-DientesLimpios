using DientesLimpios.Aplicacion.Contratos.Persistencia;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Aplicacion.Excepciones;
using DientesLimpios.Aplicacion.Utilidades.Mediador;

namespace DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.BorrarConsultorio
{
    public class CasoDeUsoBorrarConsultorio : IRequestHandler<ComandoBorrarConsultorio>
    {
        private readonly IRepositorioConsultorios _repositorio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CasoDeUsoBorrarConsultorio(IRepositorioConsultorios repositorio, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorio = repositorio;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public async Task Handle(ComandoBorrarConsultorio request)
        {
            var consultorio = await _repositorio.ObtenerPorId(request.Id);

            if (consultorio is null)
            {
                throw new ExcepcionNoEncontrado();
            }

            try
            {
                await _repositorio.Eliminar(consultorio);
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
