using System.Text.Json.Serialization;

namespace MyWebApp.Models;

public class BaseResponse
{
    [JsonPropertyOrder(-2)]
    public int StatusCode { get; set; }
    [JsonPropertyOrder(-1)]
    public string Message { get; set; }
}