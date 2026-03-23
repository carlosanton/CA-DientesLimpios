namespace DientesLimpios.Aplicacion.Utilidades.Mediador
{
    public interface IRequest<TResponse>
    {
    }

    // Para casos de uso que no devuelven datos, se puede usar esta interfaz sin tipo de respuesta
    public interface IRequest
    {
    }
}
