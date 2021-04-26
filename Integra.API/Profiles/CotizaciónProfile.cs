using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;
using AutoMapper;

namespace Integra.API.Profiles
{
	public class CotizaciónProfile : Profile
	{
		public CotizaciónProfile()
		{
			CreateMap<Cotización, CotizaciónSinLíneasDto>();
			CreateMap<Cotización, CotizaciónDto>();
			CreateMap<PaginatedList<Cotización>, PaginatedList<CotizaciónDto>>();
		}
	}
}
