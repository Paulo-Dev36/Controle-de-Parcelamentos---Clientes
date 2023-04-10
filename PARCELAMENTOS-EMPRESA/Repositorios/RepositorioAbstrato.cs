using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Projeto_Construir_Desktops
{
    public abstract class RepositorioAbstrato<T> where T : IEntidade
    {
        public abstract void Add(T x);
        public abstract void Update(T x);
        public abstract void Remove(T x);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
