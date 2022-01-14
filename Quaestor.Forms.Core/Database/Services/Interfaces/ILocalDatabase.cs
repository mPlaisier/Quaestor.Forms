using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quaestor.Forms.Core.Database.Models.Interfaces;

namespace Quaestor.Forms.Core.Database.Services
{
    public interface IDatabaseService
    {
        Task<bool> DeleteAsync<T>(T data) where T : class, new();
        Task<T> GetAsync<T>(int id) where T : class, IBaseDb, new();
        Task<List<T>> GetListAsync<T>() where T : class, new();
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<bool> SaveAsync<T>(T data) where T : class, IBaseDb, new();
    }
}