using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using RDS.ExpenseTracker.Api.Auth;


namespace RDS.ExpenseTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService service, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactionDto>))]
        public async Task<IResult> Get(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate
            )
        {
            try
            {
                var results = await _service.GetTransactions(transactions => transactions
                    .Where(t => (t.Date <= (toDate ?? DateTime.MaxValue)) && (t.Date >= (fromDate ?? DateTime.MinValue))));
                var dtos = _mapper.Map<IEnumerable<TransactionDto>>(results);
                return Results.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionDto))]
        public async Task<IResult> Get(int id)
        {
            try
            {
                var transaction = await _service.GetTransaction(id);
                var dto = _mapper.Map<TransactionDto>(transaction);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IResult> Post([FromBody] TransactionDto dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);

            try
            {
                var id = await _service.AddTransaction(transaction);
                return Results.Ok(id);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [HttpPost("/reset")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IResult> Reset([FromBody] IEnumerable<TransactionDto> dtos)
        {
            var transactions = _mapper.Map<IEnumerable<Transaction>>(dtos);

            try
            {
                await _service.ResetTransactions(transactions);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionDto))]
        public async Task<IResult> Put(int id, [FromBody] TransactionDto dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);

            try
            {
                await _service.UpdateTransaction(transaction);
                return Results.Ok(transaction);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [AuthorizeForScopes(Scopes = [Scopes.TransactionResetter])]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IResult> Put([FromBody] IEnumerable<TransactionDto> transactionDtos)
        {
            var transactions = _mapper.Map<IEnumerable<Transaction>>(transactionDtos);
            try
            {
                await _service.ResetTransactions(transactions);
                return Results.Ok();
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
                await _service.DeleteTransaction(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }
    }
}
