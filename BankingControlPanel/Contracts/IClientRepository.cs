using BankingControlPanel.DTOs;
using BankingControlPanel.Models;

namespace BankingControlPanel.Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClients(ClientParameters parameters);
        Task<Client> GetClientById(int id);
        Task AddClient(Client client);
        Task UpdateClient(Client client);
        Task DeleteClient(int id);
        Task<bool> EmailExists(string email);
        Task<bool> PersonalIdExists(string personalId);

        Task<bool> EmailExistsForOtherClient(int clientId, string email);
        Task<bool> PersonalIdExistsForOtherClient(int clientId, string personalId);

    }

}

