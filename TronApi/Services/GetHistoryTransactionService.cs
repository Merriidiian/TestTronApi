using TronApi.Exceptions;
using TronApi.Models;

namespace TronApi.Services;

public static class GetHistoryTransactionService
{
    const int accountLength = 34;
    public static HttpRequestMessage GetHttpConnectionClientAsync(string address)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://nile.trongrid.io//v1/accounts/{address}/transactions/trc20"),
            Headers =
            {
                { "accept", "application/json" },
            }
        };
        return request;
    }

    public static string GetAccountAddress()
    {
        Console.WriteLine("Ввод адреса");
        //TNPns1Wa3NZYozYKTJvsEshk6FS4opWgnf
        var address = Console.ReadLine();
        if (address.Length != accountLength)
        {
            throw new AddressFormatException("Неверный формат адреса");
        }

        return address;
    }

    public static DateTime GetDataTimeRange(string dateBeginOrEnd)
    {
        Console.WriteLine($"Ввод {dateBeginOrEnd} даты в виде dd.mm.yyyy");
        var dateBeginConsoleReadLine = Console.ReadLine();
        var date = DateTime.Parse(dateBeginConsoleReadLine);
        if (date > DateTime.Now)
        {
            throw new DataTimeException("Неверно введена дата");
        }
        return date;
    }

    public static DateTime ParseToNormalDataTimeType(Data oneTransaction)
    {
        var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        date = date.AddSeconds(int.Parse(oneTransaction.block_timestamp.Remove(9, 3)));
        return date;
    }

    public static void WriteTransactionInfo(Data oneTransaction)
    {
        Console.WriteLine("ID: " + oneTransaction.transaction_id);
        Console.WriteLine("DATE: " + ParseToNormalDataTimeType(oneTransaction));
        Console.WriteLine("FROM: " + oneTransaction.from);
        Console.WriteLine("TO: " + oneTransaction.to);
        Console.WriteLine("ADDRESS: " + oneTransaction.token_info.address);
        Console.WriteLine("DECIMALS: " + oneTransaction.token_info.decimals);
        Console.WriteLine("NAME: " + oneTransaction.token_info.name);
        Console.WriteLine("SYMBOL: " + oneTransaction.token_info.symbol);
        Console.WriteLine("TYPE: " + oneTransaction.type);
        Console.WriteLine("VALUE: " + oneTransaction.value.Remove(oneTransaction.value.Length - 6, 6) + " \n");
    }
}