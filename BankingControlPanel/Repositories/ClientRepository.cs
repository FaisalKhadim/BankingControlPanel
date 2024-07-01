using BankingControlPanel.Contracts;
using BankingControlPanel.Data;
using BankingControlPanel.DTOs;
using BankingControlPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingControlPanel.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetClients(ClientParameters parameters)
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Accounts)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Accounts)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> AddClient(Client client)
        {
            if (client.Address != null)
            {
                // Ensure Address exists in the database or attach it
                var existingAddress = await _context.Addresses.FindAsync(client.Address.Id);
                if (existingAddress != null)
                {
                    _context.Entry(existingAddress).State = EntityState.Modified;
                }
                else
                {
                    _context.Addresses.Add(client.Address);
                }
            }
            if (client.Accounts != null)
            {
                foreach (var account in client.Accounts)
                {
                    account.ClientId = client.Id; // Ensure the accounts have the correct client ID
                    await _context.Accounts.AddAsync(account);
                }
            }

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Clients.AnyAsync(c => c.Email == email);
        }

        public async Task<bool> PersonalIdExists(string personalId)
        {
            return await _context.Clients.AnyAsync(c => c.PersonalId == personalId);
        }

        public async Task<bool> EmailExistsForOtherClient(int clientId, string email)
        {
            return await _context.Clients.AnyAsync(c => c.Id != clientId && c.Email == email);
        }

        public async Task<bool> PersonalIdExistsForOtherClient(int clientId, string personalId)
        {
            return await _context.Clients.AnyAsync(c => c.Id != clientId && c.PersonalId == personalId);
        }

        public async Task<bool> AccountNumberExists(string accountNumber)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber);
        }
    }
}