using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.Dtos
{
    public class ReadState
    {
        public ReadState(State state)
        {
            Id = state.Id;
            Name = state.Name;
            Initials = state.Initials;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
    }
}
