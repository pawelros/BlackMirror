namespace BlackMirror.Models
{
    public class UserId : StringId<UserId>
    {
        public UserId(string id) : base(id)
        {
        }
    }
}