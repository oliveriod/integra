using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;
using AutoMapper;

namespace Integra.API.Profiles
{
	public class ClienteProfile : Profile
	{
		public ClienteProfile()
		{
			CreateMap<Cliente, ClienteDto>();
			CreateMap<PaginatedList<Cliente>, PaginatedList<ClienteDto>>();
		}
	}
}
