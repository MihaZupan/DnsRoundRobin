using DnsRoundRobin;
using System.Net;
using System.Net.Sockets;

var roundRobinConnector = new DnsRoundRobinConnector(
    dnsRefreshInterval: TimeSpan.FromSeconds(10),
    endpointConnectTimeout: TimeSpan.FromSeconds(5));

const string TestHost = "httpbin.org";

Console.WriteLine($"{TestHost} resolved to:");
foreach (IPAddress address in await Dns.GetHostAddressesAsync(TestHost))
{
    Console.WriteLine(address);
}
Console.WriteLine();

for (int i = 0; i < 100; i++)
{
    using var handler = new SocketsHttpHandler
    {
        ConnectCallback = async (context, cancellation) =>
        {
            Socket socket = await roundRobinConnector.ConnectAsync(context.DnsEndPoint, cancellation);

            Console.WriteLine($"Connected to {socket.RemoteEndPoint}");

            return new NetworkStream(socket, ownsSocket: true);
        }
    };

    using var client = new HttpClient(handler);

    await client.GetStringAsync($"https://{TestHost}");

    await Task.Delay(1000);
}