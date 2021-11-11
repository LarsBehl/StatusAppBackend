namespace StatusAppBackend.Database.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
    }
}