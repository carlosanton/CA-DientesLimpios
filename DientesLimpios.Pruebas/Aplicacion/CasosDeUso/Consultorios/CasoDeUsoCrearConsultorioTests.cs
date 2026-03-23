using DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Comandos.CrearConsultorio;
using DientesLimpios.Aplicacion.Contratos.Persistencia;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Dominio.Entidades;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DientesLimpios.Pruebas.Aplicacion.CasosDeUso.Consultorios
{
    [TestClass]
    public class CasoDeUsoCrearConsultorioTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IRepositorioConsultorios _repositorio;
        private IUnidadDeTrabajo _unidadDeTrabajo;
        private CasoDeUsoCrearConsultorio _casoDeUso;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            _repositorio = Substitute.For<IRepositorioConsultorios>();
            _unidadDeTrabajo = Substitute.For<IUnidadDeTrabajo>();
            _casoDeUso = new CasoDeUsoCrearConsultorio(_repositorio, _unidadDeTrabajo);
        }

        [TestMethod]
        public async Task Handle_ComandoValido_ObtenemosIdConsultorio()
        {
            var comando = new ComandoCrearConsultorio { Nombre = "Consultorio A" };

            var consultrioCreado = new Consultorio("Consultrio A");
            _repositorio.Agregar(Arg.Any<Consultorio>()).Returns(consultrioCreado);

            var resultado = await _casoDeUso.Handle(comando);

            await _repositorio.Received(1).Agregar(Arg.Any<Consultorio>());
            await _unidadDeTrabajo.Received(1).Persistir();

            Assert.AreNotEqual(Guid.Empty, resultado);
        }

        [TestMethod]
        public async Task Handle_CuandoHayError_HacemosRollback()
        {
            var comando = new ComandoCrearConsultorio { Nombre = "Consultorio A" };
            _repositorio.Agregar(Arg.Any<Consultorio>()).Throws<Exception>();

            await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await _casoDeUso.Handle(comando);
            });

            await _unidadDeTrabajo.Received(1).Revertir();

        }
    }
}
