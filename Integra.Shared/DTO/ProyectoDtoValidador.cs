using FluentValidation;

namespace Integra.Shared.DTO
{
	public class ProyectoDtoValidador : AbstractValidator<ProyectoDto>
	{
		public ProyectoDtoValidador()
		{
			CascadeMode = CascadeMode.Stop;
			RuleFor(proyecto => proyecto.Nombre).NotEmpty().WithMessage("Se requiere un nombre para la cotización.")
				.Length(5, 100).WithMessage("El nombre debe tener entre 5 y 100 caracteres.");

			RuleFor(proyecto => proyecto.Código).NotEmpty().WithMessage("Se requiere el código de la cotización.")
				.Length(3, 20).WithMessage("El código debe tener entre 3 y 20 caracteres.");

			RuleFor(proyecto => proyecto.ClienteId).NotEmpty().WithMessage("El proyecto debe estar asociado a un cliente.");

		}
	}
}
