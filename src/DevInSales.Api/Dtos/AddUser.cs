namespace DevInSales.Api.Dtos
{
    public record AddUser(string Name, string Username, string Email, string Password, DateTime BirthDate) { }
}