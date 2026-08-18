
namespace MovieCatalogue.Domain.Entities
{
    public class CastMember
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public CastMember(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Cast member name cannot be empty.", nameof(name));

            Id = id;
            Name = name;
        }
    }
}
