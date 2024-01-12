using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;


namespace eHandbook.Core.Application.Business.Contracts
{
    public interface IService<T> where T : class
    {
       public Task<T> GetManualByIdAsync(int id);
    }
}