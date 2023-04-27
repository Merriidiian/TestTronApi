namespace TronApi.Models;

public class TransactionInfoModel
{
    public Data[] data { get; set; }
    public bool success { get; set; }
    public Meta meta { get; set; }
}

public class Data
{
    public string transaction_id { get; set; }
    public TokenInfo token_info { get; set; }
    public string block_timestamp { get; set; }
    public string from { get; set; }
    public string to { get; set; }
    public string type { get; set; }
    public string value { get; set; }
}

public class TokenInfo
{
    public string symbol { get; set; }
    public string address { get; set; }
    public string decimals { get; set; }
    public string name { get; set; }
}

public class Meta
{
    public string at { get; set; }
    public string page_size { get; set; }
}
