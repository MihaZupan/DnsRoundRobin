# DnsRoundRobin [![NuGet](https://img.shields.io/nuget/v/DnsRoundRobin.svg)](https://www.nuget.org/packages/DnsRoundRobin/)

A helper for connecting Sockets to endpoints in a round-robin fashion.

```c#
var handler = new SocketsHttpHandler
{
    ConnectCallback = async (context, cancellation) =>
    {
        Socket socket = await DnsRoundRobinConnector.Shared.ConnectAsync(context.DnsEndPoint, cancellation);

        return new NetworkStream(socket, ownsSocket: true);
    }
};
```

You can configure things like the dns refresh interval or per-endpoint connection timeouts.

```c#
private static readonly DnsRoundRobinConnector s_roundRobinConnector = new(
    dnsRefreshInterval: TimeSpan.FromSeconds(10),
    endpointConnectTimeout: TimeSpan.FromSeconds(5));
```