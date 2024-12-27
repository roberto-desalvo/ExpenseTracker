using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ExpenseTrackerContext _context;

        public CategoryService(IMapper mapper, ExpenseTrackerContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<Category>>(categories);
        }

        public async Task<Category> GetDefaultCategory()
        {
            var entity = await _context.Categories.AsQueryable()
                .Where(c => c.Name.ToLower().Trim() == "default")
                .FirstOrDefaultAsync();

            return _mapper.Map<Category>(entity);    
        }
    }
}
