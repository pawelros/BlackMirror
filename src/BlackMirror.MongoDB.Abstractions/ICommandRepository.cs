namespace BlackMirror.MongoDB.Abstractions
{
    using BlackMirror.Interfaces;

    public interface ICommandRepository<out M, in D>
        where M : IModel
        where D : IDto
    {
        M Add(D dto);

        void Update(string id, D dto);

        void Delete(string id);
    }
}