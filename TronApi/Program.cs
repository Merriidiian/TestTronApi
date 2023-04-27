using System.Net.Http.Headers;

public class Program
{
    public static async Task Main(string[] args)
    {
        await GetHistoryTransaction();
    }

    // 4. Получение истории операций
    static async Task GetHistoryTransaction()
    {
        string address = Console.ReadLine();
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.shasta.trongrid.io/v1/accounts/{address}/transactions"),
            Headers =
            {
                { "accept", "application/json" },
            },
        };
        using var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var exceptionResponse = response.Content.ReadAsStringAsync();
            string exceptionMessage = await exceptionResponse;
            Console.WriteLine(exceptionMessage);
        }
        else
        {
            var bodySuccess =response.Content.ReadAsStringAsync();
            string bodyMessage = await bodySuccess;
            Console.WriteLine(bodyMessage);
        }
    }
}