using System.Text.Json.Nodes;
using System.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TronApi.Exceptions;
using TronApi.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        await GetHistoryTransaction();
    }

    // 4. Получение истории операций
    static async Task GetHistoryTransaction()
    {
        TransactionInfoModel transactionInfoModel;
        Console.WriteLine("Ввод адреса");
        //TNPns1Wa3NZYozYKTJvsEshk6FS4opWgnf
        string address = Console.ReadLine();
        if (address.Length != 34)
        {
            throw new AddressFormatException("Неверный формат адреса");
        }
        Console.WriteLine("Ввод нижнего прога даты в виде dd.mm.yyyy");
        string dateBeginConsoleReadLine = Console.ReadLine();
        DateTime dateBegin = DateTime.Parse(dateBeginConsoleReadLine);
        if (dateBegin > DateTime.Now)
        {
            throw new DataTimeException("Неверно введена дата");
        }
        Console.WriteLine("Ввод верхнего прога даты в виде dd.mm.yyyy");
        string dateEndConsoleReadLine = Console.ReadLine();
        DateTime dateEnd = DateTime.Parse(dateEndConsoleReadLine);
        if (dateEnd > DateTime.Now)
        {
            throw new DataTimeException("Неверно введена дата");
        }
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://nile.trongrid.io//v1/accounts/{address}/transactions/trc20"),
            Headers =
            {
                { "accept", "application/json" },
            },
        };
        var response = await client.SendAsync(request);
        var exceptionResponse = response.Content.ReadAsStringAsync();
        JObject message = JObject.Parse(exceptionResponse.Result);
        if (message.ToObject<ErrorAccountModel>().statusCode == 400)
        {
            throw new AccountException("Акканут не найден");
        }
        transactionInfoModel = message.ToObject<TransactionInfoModel>();
        for (int i = 0; i < transactionInfoModel.data.Length; i++)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds(int.Parse(transactionInfoModel.data[i].block_timestamp.Remove(9, 3)));
            int resultCompareDataBegin = DateTime.Compare(date, dateBegin);
            int resultCompareDataEnd = DateTime.Compare(date, dateEnd);
            if (resultCompareDataBegin >= 0 && resultCompareDataEnd <= 0)
            {
                Console.WriteLine("ID: " + transactionInfoModel.data[i].transaction_id);
                Console.WriteLine("DATE: " + date);
                Console.WriteLine("FROM: " + transactionInfoModel.data[i].from);
                Console.WriteLine("TO: "+transactionInfoModel.data[i].to);
                Console.WriteLine("ADDRESS: " + transactionInfoModel.data[i].token_info.address);
                Console.WriteLine("DECIMALS: " + transactionInfoModel.data[i].token_info.decimals);
                Console.WriteLine("NAME: " + transactionInfoModel.data[i].token_info.name);
                Console.WriteLine("SYMBOL: " + transactionInfoModel.data[i].token_info.symbol);
                Console.WriteLine("TYPE: "+transactionInfoModel.data[i].type);
                Console.WriteLine("VALUE: "+transactionInfoModel.data[i].value.Remove(transactionInfoModel.data[i].value.Length-6,6));
            }
            else
            {
                throw new TransactionException("Транзакций не найдено");
            }
        }
    }
}