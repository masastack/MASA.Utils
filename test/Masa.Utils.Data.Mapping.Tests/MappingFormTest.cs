﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Data.Mapping.Tests;

[TestClass]
public class MappingFormTest: BaseMappingTest
{
    [TestMethod]
    public void TestUseShareModeReturnMapRuleCountIs1()
    {
        var request = new CreateUserRequest()
        {
            Name = "Jim",
        };
        _mapper.Map<CreateUserRequest, User>(request);
        Assert.IsTrue(TypeAdapterConfig.GlobalSettings.RuleMap.Count == 1);
    }
}
