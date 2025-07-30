using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Core
{
    public interface IUnitOfWork :IAsyncDisposable 
    {
        //Function Signatures for Unit of Work Pattern
        IGenericRepositort<TEntity> Repository<TEntity>() where TEntity:BaseEntity; // Get Repository of Type T
        Task<int> CompleteAsync(); // Save Changes to DB
        //Task DisposeAsync(); // Dispose the Unit of Work
        //Task<bool> HasChanges(); // Check if there are any changes to save
        //Task RollbackAsync(); // Rollback the transaction if needed
        //Task CommitAsync(); // Commit the transaction if needed
    }
}
