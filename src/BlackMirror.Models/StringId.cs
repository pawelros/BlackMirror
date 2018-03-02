namespace BlackMirror.Models
{
    public class StringId<TIdClass> : ValueObject<TIdClass>
        where TIdClass : StringId<TIdClass>
    {
        public StringId(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }

        public override string ToString()
        {
            return this.Id;
        }
    }
}