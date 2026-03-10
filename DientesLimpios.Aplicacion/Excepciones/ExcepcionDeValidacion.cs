using FluentValidation.Results;

namespace DientesLimpios.Aplicacion.Excepciones
{
    public class ExcepcionDeValidacion : Exception
    {
        public List<string> ErroresDeValidacion { get; set; } = [];

        public ExcepcionDeValidacion(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                ErroresDeValidacion.Add(error.ErrorMessage);
            }
        }
    }
}
