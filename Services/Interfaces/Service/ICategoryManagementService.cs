using Domain.Entities;
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
        Task Create(Dictionary<string, string> categoryDictionary);
        Dictionary<string, object> GetCategory(int categoryId);
    }
}
