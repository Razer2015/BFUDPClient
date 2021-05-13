using Battlelog;
using BFUDPClient.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdpClient = NetCoreServer.UdpClient;

namespace BFUDPClient
{
    class GameClient : UdpClient
    {
        public class DataEventArgs : EventArgs
        {
            public EndPoint Endpoint { get; set; }
            public byte[] Buffer { get; set; }
            public long Offset { get; set; }
            public long Size { get; set; }
        }

        public class ErrorEventArgs : EventArgs
        {
            public SocketError Error { get; set; }
        }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<DataEventArgs> Received;
        public event EventHandler<ErrorEventArgs> Error;

        public GameClient(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            Disconnect();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            Connected?.Invoke(this, null);

            Socket.SendTimeout = 5 * 1000;
            Socket.ReceiveTimeout = 5 * 1000;

            Console.WriteLine($"Echo UDP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Disconnected?.Invoke(this, null);

            Console.WriteLine($"Echo UDP client disconnected a session with Id {Id}");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                Connect();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            Received?.Invoke(this, new DataEventArgs {
                Endpoint = endpoint,
                Buffer = buffer,
                Offset = offset,
                Size = size
            });

            //Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
        }

        protected override void OnError(SocketError error)
        {
            Error?.Invoke(this, new ErrorEventArgs {
                Error = error
            });

            Console.WriteLine($"Echo UDP client caught an error with code {error}");
        }

        public ServerInfo GetServerInfo(long gameId)
        {
            try
            {
                var gameIdBuffer = BitConverter.GetBytes(gameId);
                Array.Reverse(gameIdBuffer);

                return GetServerInfo(gameIdBuffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private ServerInfo GetServerInfo(byte[] gameId)
        {
            using var ms = new MemoryStream();
            ms.Write(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x51, 0x50, 0x5f });
            ms.Write(gameId);

            // Get challenge
            byte[] challenge;
            {
                Send(ms.ToArray());

                var endpoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
                byte[] response = new byte[1024];
                var size = Receive(ref endpoint, response);

                challenge = response[(size > 4 ? 8 : 0)..(int)size];
            }

            // Get serverinfo
            {
                ms.Write(challenge);
                Send(ms.ToArray());

                var endpoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
                byte[] response = new byte[4096];
                var size = Receive(ref endpoint, response);

                var serverInfoData = response[0..(int)size];
#if DEBUG
                File.WriteAllBytes("serverData.bin", serverInfoData);
#endif
                return new ServerInfo(serverInfoData);
            }
        }

        public byte[] GetServerInfoBytes(long gameId)
        {
            try
            {
                var gameIdBuffer = BitConverter.GetBytes(gameId);
                Array.Reverse(gameIdBuffer);

                return GetServerInfoBytes(gameIdBuffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private byte[] GetServerInfoBytes(byte[] gameId)
        {
            using var ms = new MemoryStream();
            ms.Write(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x51, 0x50, 0x5f });
            ms.Write(gameId);

            // Get challenge
            byte[] challenge;
            {
                Send(ms.ToArray());

                var endpoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
                byte[] response = new byte[1024];
                var size = Receive(ref endpoint, response);

                challenge = response[(size > 4 ? 8 : 0)..(int)size];
            }

            // Get serverinfo
            {
                ms.Write(challenge);
                Send(ms.ToArray());

                var endpoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
                byte[] response = new byte[4096];
                var size = Receive(ref endpoint, response);

                var serverInfoData = response[0..(int)size];
#if DEBUG
                File.WriteAllBytes("serverData.bin", serverInfoData);
#endif
                return serverInfoData;
            }
        }

        private bool _stop;
    }

    class Program
    {
        static GameClient client;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: BFUDPClient.exe <serverGuid>");
                return;
            }

            var serverShow = BattlelogClient.GetServerShow(args[0]);

            // UDP server address
            string address = serverShow.ip;

            // UDP server port
            int port = serverShow.port;

            // GameId
            long gameId = serverShow.gameId;

            Console.WriteLine($"UDP server address: {address}");
            Console.WriteLine($"UDP server port: {port}");

            Console.WriteLine();

            // Create a new UDP client
            client = new GameClient(address, port);

            client.Connected += Client_Connected;
            client.Disconnected += Client_Disconnected;
            client.Received += Client_Received;
            client.Error += Client_Error;

            // Connect the client
            Console.Write("Client connecting...");
            client.Connect();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to fetch server info or 'quit'/'q' to stop the client or '!' to reconnect the client...");

            // Keep checking player counts
            //while (true)
            //{
            //    byte[] buffer = Array.Empty<byte>();
            //    try
            //    {
            //        buffer = client.GetServerInfoBytes(gameId);
            //        var serverInfo = new ServerInfo(buffer);
            //        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} | Queue: {serverInfo.WaitingPlayers,2} - Players: {serverInfo.GetTotalPlayers(),2} - Joinig: {serverInfo.GetJoiningPlayers(),2}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} | {ex.Message}");
            //        File.WriteAllBytes($"serverData_error_{DateTime.Now.ToFileTime()}.bin", buffer);
            //    }
            //    Thread.Sleep(2 * 1000);
            //}

            // Perform text input
            while (true)
            {
                string line = Console.ReadLine();
                if (line.Equals("quit", StringComparison.OrdinalIgnoreCase) || line.Equals("q", StringComparison.OrdinalIgnoreCase))
                    break;

                // Disconnect the client
                if (line == "!")
                {
                    Console.Write("Client disconnecting...");
                    client.Disconnect();
                    Console.WriteLine("Done!");
                    continue;
                }

                // 
                //File.WriteAllBytes("serverData.bin", client.GetServerInfo(gameId));
                //Console.WriteLine(JsonConvert.SerializeObject(client.GetServerInfo(gameId)));
                Console.WriteLine(client.GetServerInfo(gameId).ToString());
            }

            // Disconnect the client
            Console.Write("Client disconnecting...");
            client.DisconnectAndStop();
            Console.WriteLine("Done!");
        }

        private static void Client_Connected(object sender, EventArgs e) { }
        private static void Client_Disconnected(object sender, EventArgs e) { }
        private static void Client_Received(object sender, GameClient.DataEventArgs e) { }
        private static void Client_Error(object sender, GameClient.ErrorEventArgs e) { }
    }
}
