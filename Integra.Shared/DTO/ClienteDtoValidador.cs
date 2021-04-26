using FluentValidation;

namespace Integra.Shared.DTO
{
	public class ClienteDtoValidador : AbstractValidator<ClienteDto>
	{
		public ClienteDtoValidador()
		{
			CascadeMode = CascadeMode.Stop;

			RuleFor(cliente => cliente.Nombre).NotEmpty().WithMessage("El nombre es requerido")
				.Length(5, 50).WithMessage("El nombre debe tener entre 2 y 50 letras.");
			RuleFor(cliente => cliente.Email).EmailAddress();


		}
	}
}
