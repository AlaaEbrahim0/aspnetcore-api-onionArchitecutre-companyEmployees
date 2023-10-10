using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	protected RepositoryContext RepositoryContext;

	protected RepositoryBase(RepositoryContext repositoryContext)
	{
		RepositoryContext = repositoryContext;
	}

	public void Create(T entity)
		=> RepositoryContext.Set<T>().Add(entity);

	public void Delete(T entity)
		=> RepositoryContext.Set<T>().Remove(entity);

	public void Update(T entity)
		=> RepositoryContext.Set<T>().Update(entity);

	public IQueryable<T> FindAll(bool trackChanges)
	{
		return !trackChanges ? RepositoryContext.Set<T>() : RepositoryContext.Set<T>().AsNoTracking();
	}

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> conition, bool trackChanges)
	{
		if (trackChanges)
		{
			return RepositoryContext.Set<T>().Where(conition);
		}
		else
		{
			return RepositoryContext.Set<T>()
				.AsNoTracking()
				.Where(conition);
		}
	}

}
