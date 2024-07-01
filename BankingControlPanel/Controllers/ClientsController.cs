using BankingControlPanel.Contracts;
using BankingControlPanel.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingControlPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;
        private readonly IValidator<ClientDto> _validator;

        public ClientsController(IClientService service, IValidator<ClientDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddClient(ClientDto clientDto)
        {
            var validationError = await ValidateChecksForAdd(clientDto);
            if (validationError != null)
            {
                return validationError;
            }

            var client = await _service.AddClient(clientDto);
            return Ok(client);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _service.GetClientById(id);
            if (client == null)
            {
                return NotFound("No Record Found");
            }
            await _service.DeleteClient(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _service.GetClientById(id);
            if (client == null)
            {
                return NotFound("No record found with this client id");
            }
            return Ok(client);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetClients([FromQuery] ClientParameters parameters)
        {
            var clients = await _service.GetClients(parameters);
            return Ok(clients);
        }

        [HttpGet("filtered")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetFilteredClients([FromQuery] ClientParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.FirstName) &&
                string.IsNullOrWhiteSpace(parameters.LastName) &&
                string.IsNullOrWhiteSpace(parameters.Email))
            {
                return BadRequest("At least one of FirstName, LastName, or Email must have a value for filtering.");
            }
            var clients = await _service.GetFilteredClients(parameters);
            var lastSearchParameters = _service.GetLastSearchParameters();
            return Ok(new { Clients = clients, LastSearchParameters = lastSearchParameters });
        }

        [HttpGet("sorted")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetSortedClients([FromQuery] ClientParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.SortBy) || string.IsNullOrWhiteSpace(parameters.SortOrder))
            {
                return BadRequest("SortBy and SortOrder must be provided for sorting.");
            }

            var clients = await _service.GetSortedClients(parameters);
            var lastSearchParameters = _service.GetLastSearchParameters();
            return Ok(new { Clients = clients, LastSearchParameters = lastSearchParameters });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
        {
            var validationError = await ValidateChecksForUpdate(id, clientDto);
            if (validationError != null)
            {
                return validationError;
            }

            await _service.UpdateClient(id, clientDto);

            return NoContent();
        }

        private async Task<IActionResult> ValidateChecksForAdd(ClientDto clientDto)
        {
            var validationResult = await _validator.ValidateAsync(clientDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var existingEmail = await _service.EmailExists(clientDto.Email);
            if (existingEmail)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var existingPersonalId = await _service.PersonalIdExists(clientDto.PersonalId);
            if (existingPersonalId)
            {
                return BadRequest(new { message = "Personal ID already exists" });
            }

            if (clientDto.Accounts != null)
            {
                foreach (var account in clientDto.Accounts)
                {
                    if (account.Balance < 0)
                    {
                        return BadRequest(new { message = "Balance cannot be negative" });
                    }

                    var existingAccountNumber = await _service.AccountNumberExists(account.AccountNumber);
                    if (existingAccountNumber)
                    {
                        return BadRequest(new { message = $"Account number {account.AccountNumber} already exists" });
                    }
                }
            }
            return null;
        }

        private async Task<IActionResult> ValidateChecksForUpdate(int id, ClientDto clientDto)
        {
            var validationResult = await _validator.ValidateAsync(clientDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var existingPersonalIdForOther = await _service.PersonalIdExistsForOtherClient(id, clientDto.PersonalId);
            if (existingPersonalIdForOther)
            {
                return BadRequest(new { message = "Personal ID already exists for other client" });
            }
            var existingEmailForOther = await _service.EmailExistsForOtherClient(id, clientDto.Email);
            if (existingEmailForOther)
            {
                return BadRequest(new { message = "Email already exists for other client" });
            }

            if (clientDto.Accounts != null && clientDto.Accounts.Any(account => account.Balance < 0))
            {
                return BadRequest(new { message = "Balance cannot be negative" });
            }

            return null;
        }
    }
}