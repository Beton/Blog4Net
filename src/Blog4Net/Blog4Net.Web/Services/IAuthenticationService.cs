namespace Blog4Net.Web.Services
{
    public interface IAuthenticationService
    {
        bool IsLogged { get; }

        bool LogOn(string username, string password); 
        void LogOff();        
    }
}