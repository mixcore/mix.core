using System.Text.Json.Serialization;

namespace Mix.OAuth.OauthRequest;

/// <summary>
/// Object define the token introspection request.
/// </summary>
public class TokenIntrospectionRequest
{
    /// <summary>
    /// Get or set token.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    /// Get or set token type hint.
    /// </summary>
    [JsonPropertyName("token_type_hint")]
    public string TokenTypeHint { get; set; }
}

