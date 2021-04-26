using AutoMapper;
using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;

namespace Integra.API.Profiles
{
	public class ProyectoProfile : Profile
	{
		public ProyectoProfile()
		{
			CreateMap<Proyecto, ProyectoDto>();
			CreateMap<PaginatedList<Proyecto>, PaginatedList<ProyectoDto>>();
		}
	}
}
