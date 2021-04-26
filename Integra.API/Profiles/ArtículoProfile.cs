using Integra.Shared.Base;
using Integra.Shared.Domain;
using Integra.Shared.DTO;

using AutoMapper;
using System.Collections.Generic;

namespace Integra.API.Profiles
{
	public class ArtículoProfile : Profile
	{
		public ArtículoProfile()
		{
			CreateMap<Artículo, ArtículoDto>();
			CreateMap<PaginatedList<Artículo>, PaginatedList<ArtículoDto>>();

			CreateMap<ArtículoTipo, ArtículoTipoDto>();
			CreateMap<ArtículoSubTipo, ArtículoSubTipoDto>();

		}
	}
}
