using FluentValidation;

namespace Integra.Shared.DTO
{
	public class ArtículoDtoValidador : AbstractValidator<ArtículoDto>
	{
		public ArtículoDtoValidador()
		{
			CascadeMode = CascadeMode.Stop;

			RuleFor(artículo => artículo.Código).NotEmpty().WithMessage("El código del artículo es requerido")
				.Length(3, 20).WithMessage("El código debe tener entre 3 y 20 caracteres");
			RuleFor(artículo => artículo.Nombre).NotEmpty().WithMessage("El nombre del cliente es requerido.")
				.Length(2, 50).WithMessage("El nombre debe teener entre 2 y 50 caracteres");
			RuleFor(artículo => artículo.Precio).NotEmpty().WithMessage("Se requiere el precio.")
				.GreaterThanOrEqualTo(0);

		}
	}
}
