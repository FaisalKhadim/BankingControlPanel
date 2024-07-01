using AutoMapper;
using BankingControlPanel.DTOs;
using BankingControlPanel.Models;

namespace BankingControlPanel.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<ClientDto, Client>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
        }
    }
}