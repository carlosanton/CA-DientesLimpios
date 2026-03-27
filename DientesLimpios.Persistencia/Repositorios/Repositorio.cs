using DientesLimpios.Aplicacion.Contratos.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace DientesLimpios.Persistencia.Repositorios
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly DientesLimpiosDbContext _context;

        public Repositorio(DientesLimpiosDbContext context)
        {
            _context = context;
        }

        public Task Actualizar(T entidad)
        {
            _context.Update(entidad);
            return Task.CompletedTask;
        }

        public Task<T> Agregar(T entidad)
        {
            _context.Add(entidad);
            return Task.FromResult(entidad);
        }

        public Task Eliminar(T entidad)
        {
            _context.Remove(entidad);
            return Task.CompletedTask;
        }

        public async Task<int> ObtenerCantidadTotalRegistros()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<T?> ObtenerPorId(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObtenerTodos()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
