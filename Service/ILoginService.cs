namespace ReserveBiteee.Service
{
    public interface ILoginService
    {
        bool FindUser(string username, string password);
    }
}
