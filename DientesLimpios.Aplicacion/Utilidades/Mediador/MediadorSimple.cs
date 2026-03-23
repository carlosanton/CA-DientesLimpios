using DientesLimpios.Aplicacion.Excepciones;
using FluentValidation;
using FluentValidation.Results;

namespace DientesLimpios.Aplicacion.Utilidades.Mediador
{
    public class MediadorSimple : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public MediadorSimple(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            await RealizarValidaciones(request);

            var tipoCasoDeUso = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var casoDeUso = _serviceProvider.GetService(tipoCasoDeUso);

            if (casoDeUso is null)
            {
                throw new ExcepcionDeMediador($"No se encontró un handler para {request.GetType().Name}");
            }

            var metodo = tipoCasoDeUso.GetMethod("Handle")!;

            return await (Task<TResponse>)metodo.Invoke(casoDeUso, new object[] { request })!;
        }

        public async Task Send(IRequest request)
        {
            await RealizarValidaciones(request);

            var tipoCasoDeUso = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

            var casoDeUso = _serviceProvider.GetService(tipoCasoDeUso);

            if (casoDeUso is null)
            {
                throw new ExcepcionDeMediador($"No se encontró un handler para {request.GetType().Name}");
            }

            var metodo = tipoCasoDeUso.GetMethod("Handle")!;

            await (Task)metodo.Invoke(casoDeUso, new object[] { request })!;
        }

        private async Task RealizarValidaciones(object request)
        {
            var tipoValidador = typeof(IValidator<>).MakeGenericType(request.GetType());

            var validador = _serviceProvider.GetService(tipoValidador);

            if (validador is not null)
            {
                var metodoValidar = tipoValidador.GetMethod("ValidateAsync")!;
                var tareaValidad = (Task)metodoValidar.Invoke(validador, new object[] { request, CancellationToken.None })!;

                await tareaValidad.ConfigureAwait(false);

                var resultado = tareaValidad.GetType().GetProperty("Result");
                var validationResult = (ValidationResult)resultado!.GetValue(tareaValidad)!;

                if (!validationResult.IsValid)
                {
                    throw new ExcepcionDeValidacion(validationResult);
                }
            }
        }
    }
}
