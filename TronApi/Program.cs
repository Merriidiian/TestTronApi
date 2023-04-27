using System.Transactions;
using Newtonsoft.Json.Linq;
using TronApi.Exceptions;
using TronApi.Models;
using TronApi.Services;

public class Program
{
    private const int accountLength = 34;
    private const int badRequest = 400;
    

    public static async Task Main(string[] args)
    {
        await GetHistoryTransactionAsync();
    }

    // 4. Получение истории операций
    private static async Task GetHistoryTransactionAsync()
    {
        TransactionInfoModel? transactionInfoModel;
        var response = await new HttpClient().SendAsync(
            GetHistoryTransactionService.GetHttpConnectionClientAsync(GetHistoryTransactionService
                .GetAccountAddress()));
        var dateBegin = GetHistoryTransactionService.GetDataTimeRange("начальной");
        var dateEnd = GetHistoryTransactionService.GetDataTimeRange("конечной");
        var messageResponse = response.Content.ReadAsStringAsync();
        var message = JObject.Parse(messageResponse.Result);
        if (message.ToObject<ErrorAccountModel>().statusCode == badRequest)
        {
            throw new AccountException("Аккаунт не найден");
        }
        transactionInfoModel = message.ToObject<TransactionInfoModel>();
        foreach (var oneTransaction in transactionInfoModel.data)
        {
            var normalDataTime = GetHistoryTransactionService.ParseToNormalDataTimeType(oneTransaction);
            var resultCompareDataBegin = DateTime.Compare(normalDataTime, dateBegin);
            var resultCompareDataEnd = DateTime.Compare(normalDataTime, dateEnd);
            if (resultCompareDataBegin >= 0 && resultCompareDataEnd <= 0)
            {
                GetHistoryTransactionService.WriteTransactionInfo(oneTransaction);
            }
            else
            {
                throw new TransactionException("Транзакций не найдено");
            }
        }
    }
}