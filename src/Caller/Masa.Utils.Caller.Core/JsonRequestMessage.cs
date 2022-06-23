// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Caller.Core;

public class JsonRequestMessage : IRequestMessage
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public JsonRequestMessage(CallerOptions callerOptions) => _jsonSerializerOptions = callerOptions.JsonSerializerOptions;

    public virtual Task<HttpRequestMessage> ProcessHttpRequestMessageAsync(HttpRequestMessage requestMessage)
        => Task.FromResult(requestMessage);

    public virtual async Task<HttpRequestMessage> ProcessHttpRequestMessageAsync<TRequest>(HttpRequestMessage requestMessage, TRequest data)
    {
        requestMessage = await ProcessHttpRequestMessageAsync(requestMessage);
        requestMessage.Content = JsonContent.Create(data, options: _jsonSerializerOptions);
        return requestMessage;
    }
}
