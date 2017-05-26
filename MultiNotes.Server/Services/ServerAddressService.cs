#define DEBUG_CONFIGURATION
namespace MultiNotes.Server.Services
{
    public class ServerAddressService
    {

#if DEBUG_CONFIGURATION
        public static string ServerAddress { get; } = "http://localhost:";
#else
        public static string ServerAddress { get; } ="http://217.61.4.233:8080/MultiNotes.Server/";
#endif
    }
}

