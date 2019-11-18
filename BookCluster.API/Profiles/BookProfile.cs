using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCluster.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Domain.Entities.Book, Models.Book>().ForSourceMember(x => x.AuthorId, z => z.DoNotValidate());
            CreateMap<Models.Book, Domain.Entities.Book>();
        }
    }
}
