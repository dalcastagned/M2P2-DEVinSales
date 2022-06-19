using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.Dtos
{
    public class ReadAddress
    {
        public ReadAddress(Address address)
        {
            Id = address.Id;
            Street = address.Street;
            Cep = address.Cep;
            Number = address.Number;
            Complement = address.Complement;
            City = new ReadAddressCity { Id = address.City.Id, Name = address.City.Name };
            State = new ReadCityState
            {
                Id = address.City.State.Id,
                Name = address.City.State.Name,
                Initials = address.City.State.Initials
            };
        }

        public int Id { get; set; }
        public string Street { get; set; }
        public string Cep { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public ReadAddressCity City { get; set; }
        public ReadCityState State { get; set; }
    }
}
