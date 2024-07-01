using AutoMapper;
using BankingControlPanel.Contracts;
using BankingControlPanel.DTOs;
using BankingControlPanel.Models;

namespace BankingControlPanel.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;
        private readonly List<string> _lastSearchParameters = new List<string>();


        public ClientService(IClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetClients(ClientParameters parameters)
        {
    
            var clients = await _repository.GetClients(parameters);
            return _mapper.Map<IEnumerable<ClientDto>>(clients);


        }

        public async Task<ClientDto> GetClientById(int id)
        {
            var client = await _repository.GetClientById(id);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task AddClient(ClientDto clientDto)
        {
            clientDto.Id = 0;
            var client = _mapper.Map<Client>(clientDto);
            await _repository.AddClient(client);
        }

        public async Task UpdateClient(int id, ClientDto clientDto)
        {
            var client = await _repository.GetClientById(id);
            if (client != null)
            {
                _mapper.Map(clientDto, client);
                await _repository.UpdateClient(client);
            }
        }

        public async Task DeleteClient(int id)
        {
            await _repository.DeleteClient(id);
        }

         public async Task<bool> EmailExists(string email)
        {
            return await _repository.EmailExists(email);
        }

        public async Task<bool> PersonalIdExists(string personalId)
        {
            return await _repository.PersonalIdExists(personalId);
        }
        public async Task<bool> EmailExistsForOtherClient(int clientId, string email)
        {

            return await _repository.EmailExistsForOtherClient(clientId, email);
        }

        public async Task<bool> PersonalIdExistsForOtherClient(int clientId, string personalId)
        {
            return await _repository.PersonalIdExistsForOtherClient(clientId, personalId);
        }


        public async Task<IEnumerable<ClientDto>> GetSortedClients(ClientParameters parameters)
        {
            // Add current search parameter to history
            var clients = await _repository.GetClients(parameters);
            clients = SortClients(clients, parameters.SortBy, parameters.SortOrder);
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        private IEnumerable<Client> SortClients(IEnumerable<Client> clients, string sortBy, string sortOrder)
        {
            switch (sortBy?.ToLower())
            {
                case "firstname":
                    clients = sortOrder?.ToLower() == sortOrder ?
                              clients.OrderByDescending(c => c.FirstName) :
                              clients.OrderBy(c => c.FirstName);
                    break;
                case "lastname":
                    clients = sortOrder?.ToLower() == sortOrder ?
                              clients.OrderByDescending(c => c.LastName) :
                              clients.OrderBy(c => c.LastName);
                    break;
                case "email":
                    clients = sortOrder?.ToLower() == sortOrder ?
                              clients.OrderByDescending(c => c.Email) :
                              clients.OrderBy(c => c.Email);
                    break;
                default:
                    break;
            }
            return clients;
        }

        public async Task<IEnumerable<ClientDto>> GetFilteredClients(ClientParameters parameters)
        {
            AddSearchParameter(parameters);
            var clients = await _repository.GetClients(parameters);
            clients = ApplyFilters(clients, parameters);
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }
        private IEnumerable<Client> ApplyFilters(IEnumerable<Client> clients, ClientParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.FirstName))
            {
                clients = clients.Where(c => c.FirstName.ToLower().Contains(parameters.FirstName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(parameters.LastName))
            {
                clients = clients.Where(c => c.LastName.ToLower().Contains(parameters.LastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(parameters.Email))
            {
                clients = clients.Where(c => c.Email.ToLower().Contains(parameters.Email.ToLower()));
            }
            // Add more filters for other properties as needed
            return clients;
        }
        private void AddSearchParameter(ClientParameters parameters)
        {

            string searchParameter = $"{parameters.FirstName}_{parameters.LastName}_{parameters.LastName}_{parameters.SortBy}_{parameters.SortOrder}";
            if (!_lastSearchParameters.Contains(searchParameter))
            {
                if (_lastSearchParameters.Count >= 3)
                {
                    _lastSearchParameters.RemoveAt(0); // Remove oldest if more than 3
                }
                _lastSearchParameters.Add(searchParameter);
            }
        }

        public IEnumerable<string> GetLastSearchParameters()
        {
            return _lastSearchParameters;
        }


    }

}
