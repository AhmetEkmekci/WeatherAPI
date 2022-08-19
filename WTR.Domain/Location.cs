using WTR.Abstraction.Entity;

namespace WTR.Domain
{
    public class Location : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime Created { get; set; }
    }
}