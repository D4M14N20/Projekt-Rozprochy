using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace agario
{
    internal class Client
    {
        internal static async Task Connect(string ip, Form1 form)
        {
            // Tworzymy nowego klienta HTTP
            try
            {
                // Adres IP i port serwera, z którym chcemy się połączyć
                string serverIP = ip; // Możesz zmienić ten adres IP na adres serwera
                int serverPort = 50080; // Możesz zmienić ten port na port serwera

                // Tworzymy nowe gniazdo TCP
                TcpClient client = new TcpClient(serverIP, serverPort);

                // Pobieramy strumień sieciowy dla wysyłania danych
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                await stream.ReadAsync(buffer, 0, 5);
                string text = Encoding.ASCII.GetString(buffer);
                form.Wiadomsoc = text;

                // Zamykamy połączenie
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: " + ex.Message);
            }
        }
    }
}
