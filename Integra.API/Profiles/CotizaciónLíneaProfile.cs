using Integra.Shared.Domain;
using Integra.Shared.DTO;

using AutoMapper;

namespace Integra.API.Profiles
{
	public class CotizaciónLíneaProfile : Profile
	{
		public CotizaciónLíneaProfile()
		{
			CreateMap<CotizaciónLínea, CotizaciónLíneaDto>();
		}
	}
}
