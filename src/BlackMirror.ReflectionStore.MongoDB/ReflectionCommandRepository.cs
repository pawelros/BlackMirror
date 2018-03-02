namespace BlackMirror.ReflectionStore.MongoDB
{
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using global::MongoDB.Bson;

    public class ReflectionCommandRepository : CommandRepositoryBase<Reflection, ReflectionDto>
    {
        public ReflectionCommandRepository(IWebApiConfiguration webApiConfiguration, string collectionName)
            : base(webApiConfiguration, collectionName)
        {
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override Reflection ConvertToModel(ReflectionDto dto)
        {
            var model = dto.ToReflection();
            return model;
        }
    }
}