using AutoMapper;
using BankingControlPanel.DTOs;
using BankingControlPanel.Models;

namespace BankingControlPanel.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
        }
    }

}
