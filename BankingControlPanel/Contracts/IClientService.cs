using BankingControlPanel.DTOs;
using BankingControlPanel.Models;

namespace BankingControlPanel.Contracts
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClients(ClientParameters parameters);

        Task<ClientDto> GetClientById(int id);

        Task<Client> AddClient(ClientDto clientDto);

        Task UpdateClient(int id, ClientDto clientDto);

        Task DeleteClient(int id);

        Task<bool> EmailExists(string email);

        Task<bool> PersonalIdExists(string personalId);

        Task<bool> EmailExistsForOtherClient(int clientId, string email);

        Task<bool> PersonalIdExistsForOtherClient(int clientId, string personalId);

        Task<IEnumerable<ClientDto>> GetFilteredClients(ClientParameters parameters);

        Task<IEnumerable<ClientDto>> GetSortedClients(ClientParameters parameters);

        IEnumerable<string> GetLastSearchParameters();

        Task<bool> AccountNumberExists(string accountNumber);
    }
}