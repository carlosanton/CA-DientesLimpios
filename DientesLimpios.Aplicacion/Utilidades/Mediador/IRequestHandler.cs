namespace DientesLimpios.Aplicacion.Utilidades.Mediador
{
    public interface IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }

    // Para casos de uso que no devuelven datos, se puede usar esta interfaz sin tipo de respuesta
    public interface IRequestHandler<TRequest>
            where TRequest : IRequest
    {
        Task Handle(TRequest request);
    }
}
