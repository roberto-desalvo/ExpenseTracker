using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Api.Dtos;
using Microsoft.AspNetCore.Authorization;


namespace RDS.ExpenseTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IFinancialAccountService _service;
        private readonly IMapper _mapper;

        public AccountController(IFinancialAccountService service, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FinancialAccountDto>))]
        public async Task<IResult> Get()
        {
            try
            {
                var results = await _service.GetFinancialAccounts();
                var dtos = _mapper.Map<IEnumerable<FinancialAccountDto>>(results);
                return Results.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialAccountDto))]
        public async Task<IResult> Get(int id)
        {
            try
            {
                var account = await _service.GetFinancialAccount(id);
                var dto = _mapper.Map<FinancialAccountDto>(account);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{id}/availability")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IResult> GetAvailability(int id)
        {
            try
            {
                var availability = await _service.GetAvailability(id);
                return Results.Ok(availability);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IResult> Post([FromBody] FinancialAccountDto dto)
        {
            var account = _mapper.Map<FinancialAccount>(dto);

            try
            {
                var id = await _service.AddFinancialAccount(account);
                return Results.Ok(id);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FinancialAccountDto))]
        public async Task<IResult> Put(int id, [FromBody] FinancialAccountDto dto)
        {
            var account = _mapper.Map<FinancialAccount>(dto);

            try
            {
                await _service.UpdateFinancialAccount(account);
                return Results.Ok(account);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                await _service.DeleteFinancialAccount(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }
    }
}
