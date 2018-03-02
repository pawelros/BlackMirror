namespace BlackMirror.MongoDB.Abstractions
{
    using System.Collections.Generic;
    using BlackMirror.Interfaces;

    public interface IQueryRepository<out M, in D>
        where M : IModel
        where D : IDto
    {
        IEnumerable<M> GetAll(Dictionary<string, string> filters = null);

        M GetById(string id);
    }
}
