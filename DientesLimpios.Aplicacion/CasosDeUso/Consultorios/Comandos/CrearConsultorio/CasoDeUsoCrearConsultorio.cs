using DientesLimpios.Aplicacion.Contratos.Persistencia;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Aplicacion.Utilidades.Mediador;
using DientesLimpios.Dominio.Entidades;

namespace DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.CrearConsultorio
{
    public class CasoDeUsoCrearConsultorio : IRequestHandler<ComandoCrearConsultorio, Guid>
    {
        private readonly IRepositorioConsultorios _repositorio;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public CasoDeUsoCrearConsultorio(IRepositorioConsultorios repositorio, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _repositorio = repositorio;
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public async Task<Guid> Handle(ComandoCrearConsultorio comando)
        {
            // Creamos la entidad de consultorio
            var consultorio = new Consultorio(comando.Nombre);

            try
            {
                // Guardamos el consultorio en el repositorio
                var respuesta = await _repositorio.Agregar(consultorio);

                // Persisto el cambio en la base de datos
                await _unidadDeTrabajo.Persistir();

                // Retornamos el ID del consultorio creado
                return respuesta.Id;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, revertimos cualquier cambio realizado
                await _unidadDeTrabajo.Revertir();
                throw;
            }
        }
    }
}
