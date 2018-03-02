namespace BlackMirror.HttpClient.Reflection
{
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface IReflectionClient
    {
        IReflection Add(ReflectionDto dto);
    }
}