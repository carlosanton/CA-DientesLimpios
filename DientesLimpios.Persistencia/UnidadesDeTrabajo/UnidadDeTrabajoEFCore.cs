using DientesLimpios.Aplicacion.Contratos.Persistencia;

namespace DientesLimpios.Persistencia.UnidadesDeTrabajo
{
    public class UnidadDeTrabajoEFCore : IUnidadDeTrabajo
    {
        private readonly DientesLimpiosDbContext _context;

        public UnidadDeTrabajoEFCore(DientesLimpiosDbContext context)
        {
            _context = context;
        }

        public async Task Persistir()
        {
            await _context.SaveChangesAsync();
        }

        public Task Revertir()
        {
            return Task.CompletedTask;
        }
    }
}
