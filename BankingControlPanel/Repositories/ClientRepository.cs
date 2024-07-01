using AutoMapper;
using BankingControlPanel.Contracts;
using BankingControlPanel.Data;
using BankingControlPanel.DTOs;
using BankingControlPanel.Models;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task AddClient(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
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

    }


}
