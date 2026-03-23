using DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.BorrarConsultorio;
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
    public class CasoDeUsoBorrarConsultorioTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IRepositorioConsultorios _repositorio;
        private IUnidadDeTrabajo _unidadDeTrabajo;
        private CasoDeUsoBorrarConsultorio _casoDeUso;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            _repositorio = Substitute.For<IRepositorioConsultorios>();
            _unidadDeTrabajo = Substitute.For<IUnidadDeTrabajo>();
            _casoDeUso = new CasoDeUsoBorrarConsultorio(_repositorio, _unidadDeTrabajo);
        }

        [TestMethod]
        public async Task Handle_CuandoConsultorioExiste_BorraConsultorioYPersiste()
        {
            var id = Guid.NewGuid();
            var comando = new ComandoBorrarConsultorio { Id = id };
            var consultorio = new Consultorio("Consultorio A");

            _repositorio.ObtenerPorId(id).Returns(consultorio);
            await _casoDeUso.Handle(comando);

            await _repositorio.Received(1).Eliminar(consultorio);
            await _unidadDeTrabajo.Received(1).Persistir();
        }

        [TestMethod]
        public async Task Handle_CuandoConsultorioNoExiste_LanzaExcepcionNoEncontrado()
        {
            var comando = new ComandoBorrarConsultorio { Id = Guid.NewGuid() };
            _repositorio.ObtenerPorId(comando.Id).ReturnsNull();

            await Assert.ThrowsExceptionAsync<ExcepcionNoEncontrado>(() => _casoDeUso.Handle(comando));
        }

        [TestMethod]
        [ExpectedException(typeof(ExcepcionNoEncontrado))]
        public async Task Handle_CuandoConsultorioNoExiste_LanzaExcepcionNoEncontrado2()
        {
            var comando = new ComandoBorrarConsultorio { Id = Guid.NewGuid() };
            _repositorio.ObtenerPorId(comando.Id).ReturnsNull();

            await _casoDeUso.Handle(comando);
        }

        [TestMethod]
        public async Task Handle_CuandoOcurreExcepcionAlBorrar_LlamaARevertirYLanzaExcepcion()
        {
            var id = Guid.NewGuid();
            var comando = new ComandoBorrarConsultorio { Id = id };
            var consultorio = new Consultorio("Consultorio A");

            _repositorio.ObtenerPorId(id).Returns(consultorio);
            _repositorio.Eliminar(consultorio).Throws(new InvalidOperationException("Fallo al eliminar"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _casoDeUso.Handle(comando));
            await _unidadDeTrabajo.Received(1).Revertir();
        }
    }
}
