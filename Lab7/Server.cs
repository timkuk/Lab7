using System;
using System.IO;
using System.Threading;
using SuperWebSocket;

namespace Lab7
{
    class Server
    {
        private static WebSocketServer wsServer;

        static string currency_USA = (File.ReadAllText("currency_USA.txt"));

        static void Main(string[] args)
        {
            wsServer = new WebSocketServer();
            int port = 8080;
            int switcher = 1;
            wsServer.Setup(port);
            wsServer.NewSessionConnected += WsServer_NewSessionConnected;
            wsServer.NewMessageReceived += WsServer_NewMessageReceived;
            wsServer.NewDataReceived += WsServer_NewDataReceived;
            wsServer.SessionClosed += WsServer_SessionClosed;
            wsServer.Start();
            Console.WriteLine("Server is running on port " + port);

            while(switcher != 0)
            {
                Console.Clear();

                string currency_USA = (File.ReadAllText("currency_USA.txt"));
                string currency_EUR = (File.ReadAllText("currency_EUR.txt"));
                string currency_RUB = (File.ReadAllText("currency_RUB.txt"));

                Console.WriteLine($"Exchange rate:\n");
                Console.WriteLine("1. USA\n2. EUR\n3. RUB\n0. Exit");

                switcher = Convert.ToInt32(Console.ReadLine());
                
                switch (switcher)
                {
                    case 1:
                        {
                            SendExchangeRates(currency_USA);
                            break;
                        }
                    case 2:
                        {
                            SendExchangeRates(currency_EUR);
                            break;
                        }
                    case 3:
                        {
                            SendExchangeRates(currency_RUB);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private static void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("Session closed");
        }

        private static void WsServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("Updated");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("New message received: " + value);
            if (value == "Hello server")
            {
                session.Send("Hello client");
            }
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session)
        {
            Thread.Sleep(1000);

            SendExchangeRates(currency_USA);
        }

        private static void SendExchangeRates(string currency)
        {
            foreach (WebSocketSession wss in wsServer.GetAllSessions())
            {
                wss.Send(currency);
            }
        }
    }
}
