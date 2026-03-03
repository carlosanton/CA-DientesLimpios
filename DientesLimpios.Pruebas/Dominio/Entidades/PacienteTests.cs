using DientesLimpios.Dominio.Entidades;
using DientesLimpios.Dominio.Excepciones;
using DientesLimpios.Dominio.ObjetosDeValor;

namespace DientesLimpios.Pruebas.Dominio.Entidades
{
    [TestClass]
    public class PacienteTests
    {
        [TestMethod]
        public void Constructor_NombreNulo_LanzaExcepcion()
        {
            var email = new Email("felipe@ejemplo.com");
            Assert.Throws<ExcepcionDeReglaDeNegocio>(() => new Paciente(null!, email));

        }

        [TestMethod]
        public void Constructor_EmailNulo_LanzaExcepcion()
        {
            Email email = null!;
            Assert.Throws<ExcepcionDeReglaDeNegocio>(() => new Paciente("Felipe", email));
        }
    }
}
