﻿namespace Masa.Utils.Caller.IntegratedTest.Callers;

public abstract class UserDaprCallerBase : DaprCallerBase
{
    protected override string AppId { get; set; }= "User-Service";

    protected UserDaprCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public string GetAppId() => AppId;
}
