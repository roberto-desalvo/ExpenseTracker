﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Data.Entities;
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

        public async Task<int> AddCategory(Category category)
        {
            var entity = _mapper.Map<ECategory>(category);
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteCategory(int id)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<Category>>(categories);   
        }

        public async Task<Category?> GetCategory(int id)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<Category?>(entity);
        }

        public async Task<Category> GetDefaultCategory()
        {
            var entity = await _context.Categories.AsQueryable()
                .Where(c => c.Name.ToLower().Trim() == "default")
                .FirstOrDefaultAsync();

            return _mapper.Map<Category>(entity);
        }

        public async Task UpdateCategory(Category modified)
        {
            var current = await _context.Categories.FirstOrDefaultAsync(x => x.Id == modified.Id);
            if (current != null)
            {
                _context.Entry(current).CurrentValues.SetValues(modified);
                await _context.SaveChangesAsync();
            }
        }
    }
}
