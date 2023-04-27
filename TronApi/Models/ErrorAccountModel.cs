namespace TronApi.Models;

public class ErrorAccountModel
{
    public bool success { get; set; }
    public string error { get; set; }
    public int statusCode { get; set; }
}