using System.Text.Json.Serialization;

namespace CR.LLM.Dtos.ChatGPT;

public class ChatCompletion
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = [];

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = null!;

    [JsonPropertyName("system_fingerprint")]
    public object? SystemFingerprint { get; set; }
}

public class Choice
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; } = null!;

    [JsonPropertyName("logprobs")]
    public object LogProbs { get; set; } = null!;

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = null!;
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
