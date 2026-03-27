namespace DientesLimpios.Aplicacion.Contratos.Repositorios
{
    public interface IRepositorio<T> where T : class
    {
        Task<T?> ObtenerPorId(Guid id);
        Task<IEnumerable<T>> ObtenerTodos();
        Task<int> ObtenerCantidadTotalRegistros();
        Task<T> Agregar(T entidad);
        Task Actualizar(T entidad);
        Task Eliminar(T entidad);
    }
}
