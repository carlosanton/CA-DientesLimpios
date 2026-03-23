using DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.ActualizarConsultorio;
using DientesLimpios.Aplicacion.Contratos.Persistencia;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Aplicacion.Excepciones;
using DientesLimpios.Dominio.Entidades;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace DientesLimpios.Pruebas.Aplicacion.CasosDeUso.Consultorios
{
    [TestClass]
    public class CasoDeUsoActualizarConsultorioTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IRepositorioConsultorios _repositorio;
        private IUnidadDeTrabajo _unidadDeTrabajo;
        private CasoDeUsoActualizarConsultorio _casoDeUso;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            _repositorio = Substitute.For<IRepositorioConsultorios>();
            _unidadDeTrabajo = Substitute.For<IUnidadDeTrabajo>();
            _casoDeUso = new CasoDeUsoActualizarConsultorio(_repositorio, _unidadDeTrabajo);
        }

        [TestMethod]
        public async Task Handle_CuandoConsultorioExiste_ActualizaNombreYPersiste()
        {
            var consultorio = new Consultorio("Consultorio A");
            var id = consultorio.Id;
            var comando = new ComandoActualizarConsultorio { Id = id, Nombre = "Nuevo nombre" };

            _repositorio.ObtenerPorId(id).Returns(consultorio);

            await _casoDeUso.Handle(comando);

            await _repositorio.Received(1).Actualizar(consultorio);
            await _unidadDeTrabajo.Received(1).Persistir();
        }

        [TestMethod]
        [ExpectedException(typeof(ExcepcionNoEncontrado))]
        public async Task Handle_CuandoConsultorioNoExiste_LanzaExcepcionNoEncontrado()
        {
            var comando = new ComandoActualizarConsultorio { Id = Guid.NewGuid(), Nombre = "Nombre" };
            _repositorio.ObtenerPorId(comando.Id).ReturnsNull();

            await _casoDeUso.Handle(comando);
        }

        [TestMethod]
        public async Task Handle_CuandoOcurreExcepcionAlActualizar_LlamaARevertirYLanzaExcepcion()
        {
            var consultorio = new Consultorio("Consultorio A");
            var id = consultorio.Id;
            var comando = new ComandoActualizarConsultorio { Id = id, Nombre = "Consultorio B" };

            _repositorio.ObtenerPorId(id).Returns(consultorio);
            _repositorio.Actualizar(consultorio).Throws(new InvalidOperationException("Error al actualizar"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _casoDeUso.Handle(comando);
            });
            await _unidadDeTrabajo.Received(1).Revertir();
        }
    }
}
