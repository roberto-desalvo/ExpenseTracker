using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTrackerApi.Dtos;


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
        public async Task<IResult> Get()
        {
            try
            {
                var results = await _service.GetTransactions();
                var dtos = _mapper.Map<IEnumerable<TransactionDto>>(results);
                return Results.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
