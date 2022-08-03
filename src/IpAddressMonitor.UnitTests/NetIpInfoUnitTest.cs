// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetIpInfoUnitTest.cs" company="MareMare">
// Copyright © 2022 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace IpAddressMonitor.UnitTests;

public class NetIpInfoUnitTest
{
    [Fact]
    public void NetIpInfo_GetNetIpInfos_Test()
    {
        var actual = NetIpInfo.GetNetIpInfos().ToArray();
        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
