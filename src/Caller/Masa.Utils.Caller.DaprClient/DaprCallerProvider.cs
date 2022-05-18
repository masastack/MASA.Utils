// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Net.Http.Json;

namespace Masa.Utils.Caller.DaprClient;

public class DaprCallerProvider : AbstractCallerProvider
{
    private readonly string AppId;
    private readonly IRequestMessage _requestMessage;
    private readonly IResponseMessage _responseMessage;
    private Dapr.Client.DaprClient? _daprClient;
    private Dapr.Client.DaprClient DaprClient => _daprClient ??= ServiceProvider.GetRequiredService<Dapr.Client.DaprClient>();

    public DaprCallerProvider(IServiceProvider serviceProvider, string appId) : base(serviceProvider)
    {
        AppId = appId;
        _requestMessage = serviceProvider.GetRequiredService<IRequestMessage>();
        _responseMessage = serviceProvider.GetRequiredService<IResponseMessage>();
    }

    public override async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default)
        where TResponse : default
    {
        var response = await DaprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
        return await _responseMessage.ProcessResponseAsync<TResponse>(response, cancellationToken);
    }

    public override Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string? methodName)
        => _requestMessage.ProcessHttpRequestMessageAsync(DaprClient.CreateInvokeMethodRequest(method, AppId, methodName));

    public override async Task<HttpRequestMessage> CreateRequestAsync<TRequest>(HttpMethod method, string? methodName, TRequest data)
    {
        var request = await _requestMessage.ProcessHttpRequestMessageAsync(DaprClient.CreateInvokeMethodRequest(method, AppId, methodName), data);
        request.Content ??= JsonContent.Create(data, options: DaprClient.JsonSerializerOptions);
        return request;
    }

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        => DaprClient.InvokeMethodWithResponseAsync(request, cancellationToken);

    public override Task SendGrpcAsync(string methodName, CancellationToken cancellationToken = default)
        => DaprClient.InvokeMethodGrpcAsync(AppId, methodName, cancellationToken);

    public override Task<TResponse> SendGrpcAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
        => DaprClient.InvokeMethodGrpcAsync<TResponse>(AppId, methodName, cancellationToken);

    public override Task SendGrpcAsync<TRequest>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        => DaprClient.InvokeMethodGrpcAsync(AppId, methodName, request, cancellationToken);

    public override Task<TResponse> SendGrpcAsync<TRequest, TResponse>(string methodName, TRequest request, CancellationToken cancellationToken = default)
        => DaprClient.InvokeMethodGrpcAsync<TResponse>(AppId, methodName, cancellationToken);
}
