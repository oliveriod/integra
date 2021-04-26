using Integra.Shared.Domain;
using Integra.Shared.DTO;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.API.Profiles
{
    public class VendedorProfile : Profile
    {
        public VendedorProfile()
        {
            CreateMap<Vendedor, VendedorDto>();
        }
    }
}
