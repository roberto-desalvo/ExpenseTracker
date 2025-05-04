using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RDS.ExpenseTracker.Api.Dtos;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;


namespace RDS.ExpenseTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService service, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
        public async Task<IResult> Get()
        {
            try
            {
                var results = await _service.GetCategories();
                var dtos = _mapper.Map<IEnumerable<CategoryDto>>(results);
                return Results.Ok(dtos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex}, {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> Get(int id)
        {
            try
            {
                var category = await _service.GetCategory(id);
                if (category == null)
                {
                    return Results.NoContent();
                }

                var dto = _mapper.Map<CategoryDto>(category);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpGet("default")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IResult> GetDefault()
        {
            try
            {
                var category = await _service.GetDefaultCategory();
                if(category == null)
                {
                    return Results.NoContent();
                }

                var dto = _mapper.Map<CategoryDto>(category);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IResult> Post([FromBody] CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            try
            {
                var id = await _service.AddCategory(category);
                return Results.Ok(id);
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<IResult> Put(int id, [FromBody] CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            try
            {
                await _service.UpdateCategory(category);
                return Results.Ok(category);
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
                await _service.DeleteCategory(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem($"{ex} {ex.Message}");
            }
        }
    }
}
