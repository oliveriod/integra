using FluentValidation;

namespace Integra.Shared.DTO
{
	public class CotizaciónDtoValidador : AbstractValidator<CotizaciónDto>
	{
		public CotizaciónDtoValidador()
		{
			CascadeMode = CascadeMode.Stop;

			RuleFor(cotización => cotización.Codigo).NotEmpty().WithMessage("Se requiere el código de la cotización.")
				.Length(5, 20).WithMessage("El código debe tener entre 5 y 20 caracteres.");
			RuleFor(cotización => cotización.Total).NotEmpty().WithMessage("La cotización no puede estar vacía")
				.GreaterThan(0);
			RuleFor(cotización => cotización.ClienteId).NotEmpty().WithMessage("La cotización necesita un cliente.");

		}
	}
}
