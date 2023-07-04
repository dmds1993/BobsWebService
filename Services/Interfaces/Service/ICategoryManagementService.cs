using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Service
{
    public interface ICategoryManagementService
    {
        Task<CategoryModel> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<CategoryModel>> GetList();
        Task Create(Dictionary<string, string> categoryDictionary);
    }
}
