using DientesLimpios.Aplicacion.CasosDeUso.Consultorios.Consultas.ObtenerDetalleConsultorio;
using DientesLimpios.Aplicacion.Contratos.Repositorios;
using DientesLimpios.Aplicacion.Excepciones;
using DientesLimpios.Dominio.Entidades;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DientesLimpios.Pruebas.Aplicacion.CasosDeUso.Consultorios
{
    [TestClass]
    public class CasoDeUsoObtenerDetalleConsultorioTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IRepositorioConsultorios _repositorio;
        private CasoDeUsoObtenerDetalleConsultorio _casoDeUso;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            _repositorio = Substitute.For<IRepositorioConsultorios>();
            _casoDeUso = new CasoDeUsoObtenerDetalleConsultorio(_repositorio);
        }

        [TestMethod]
        public async Task Handle_ConsultorioExiste_RetornaDTO()
        {
            // Preparación
            var consultorio = new Consultorio("Consultorio A");
            var id = consultorio.Id;
            var consulta = new ConsultaObtenerDetalleConsultorio { Id = id };

            _repositorio.ObtenerPorId(id).Returns(consultorio);

            // Prueba
            var resultado = await _casoDeUso.Handle(consulta);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(id, resultado.Id);
            Assert.AreEqual("Consultorio A", resultado.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(ExcepcionNoEncontrado))]
        public async Task Handle_ConsultorioNoExiste_LanzaExcepcionNoEncontrado()
        {
            // Preparación
            var id = Guid.NewGuid();
            var consulta = new ConsultaObtenerDetalleConsultorio { Id = id };

            _repositorio.ObtenerPorId(id).ReturnsNull();

            // Prueba
            await _casoDeUso.Handle(consulta);
        }
    }
}
