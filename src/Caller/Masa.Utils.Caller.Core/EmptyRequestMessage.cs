// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Caller.Core;

public class EmptyRequestMessage : IRequestMessage
{
    public Task<HttpRequestMessage> ProcessHttpRequestMessageAsync(HttpRequestMessage requestMessage)
        => Task.FromResult(requestMessage);

    public Task<HttpRequestMessage> ProcessHttpRequestMessageAsync<TRequest>(HttpRequestMessage requestMessage, TRequest data)
        => Task.FromResult(requestMessage);
}
