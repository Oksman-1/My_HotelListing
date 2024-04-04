using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	protected DatabaseContext DatabaseContext;

	protected RepositoryBase(DatabaseContext databaseContext)
	{
		DatabaseContext = databaseContext;
	}

	public void Create(T entity) => DatabaseContext.Set<T>().Add(entity);	
	
	public void Delete(T entity) => DatabaseContext.Set<T>().Remove(entity);

	public EntityState dd(T entity) => DatabaseContext.Entry(entity).State;

	public IQueryable<T> FindAll(bool trackChanges) => 
		!trackChanges ? DatabaseContext.Set<T>().AsNoTracking()
		: DatabaseContext.Set<T>();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges ?       DatabaseContext.Set<T>()
		.Where(expression)
		.AsNoTracking()	: DatabaseContext.Set<T>()
		.Where(expression);

	public void Update(T entity) => DatabaseContext.Set<T>().Update(entity);	


}
