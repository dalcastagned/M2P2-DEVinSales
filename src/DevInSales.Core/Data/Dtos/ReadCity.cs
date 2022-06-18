using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.Dtos
{
    public class ReadCity
    {
        public ReadCity(City city)
        {
            Id = city.Id;
            Name = city.Name;
            State = new ReadCityState
            {
                Id = city.State.Id,
                Name = city.State.Name,
                Initials = city.State.Initials
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ReadCityState State { get; set; }
    }
}
