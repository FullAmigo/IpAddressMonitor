// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetIpInfo.cs" company="MareMare">
// Copyright © 2022 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IpAddressMonitor
{
    /// <summary>
    /// ネットワークインターフェイスアドレスに関する情報を表します。
    /// </summary>
    [DebuggerDisplay("{InterfaceName}: IP={IpAddress} Type={InterfaceType} Status={Status}")]
    public class NetIpInfo
    {
        /// <summary>
        /// <see cref="NetIpInfo" /> クラスの新しいインスタンスを初期化します。
        /// </summary>
        private NetIpInfo()
        {
        }

        /// <summary>
        /// ネットワーク インターフェイスのインデックスを取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="int" /> 型。
        /// <para>ネットワーク インターフェイスのインデックス。</para>
        /// </value>
        public int InterfaceIndex { get; private set; }

        /// <summary>
        /// ネットワーク アダプターの説明を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="string" /> 型。
        /// <para>ネットワーク アダプターの説明。</para>
        /// </value>
        public string InterfaceDescription { get; private set; } = null!;

        /// <summary>
        /// ネットワーク インターフェイスの速度を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="long" /> 型。
        /// <para>ネットワーク インターフェイスの速度。</para>
        /// </value>
        public long InterfaceLinkSpeed { get; private set; }

        /// <summary>
        /// ネットワーク アダプターの名前を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="string" /> 型。
        /// <para>ネットワーク アダプターの名前。</para>
        /// </value>
        public string InterfaceName { get; private set; } = null!;

        /// <summary>
        /// ネットワーク インターフェイスの種類を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="NetworkInterfaceType" /> 型。
        /// <para>ネットワーク インターフェイスの種類。既定値は <see cref="NetworkInterfaceType.Unknown" /> です。</para>
        /// </value>
        public NetworkInterfaceType InterfaceType { get; private set; } = NetworkInterfaceType.Unknown;

        /// <summary>
        /// インターネット プロトコル (IP: Internet Protocol) アドレスを取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="IPAddress" /> 型。
        /// <para>インターネット プロトコル (IP: Internet Protocol) アドレス。</para>
        /// </value>
        public IPAddress IpAddress { get; private set; } = null!;

        /// <summary>
        /// ネットワーク アダプターの物理アドレス (MAC アドレス) を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="PhysicalAddress" /> 型。
        /// <para>ネットワーク アダプターの物理アドレス (MAC アドレス)。</para>
        /// </value>
        public PhysicalAddress MacAddress { get; private set; } = null!;

        /// <summary>
        /// ネットワーク接続の現在の操作状態を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="OperationalStatus" /> 型。
        /// <para>ネットワーク接続の現在の操作状態。既定値は <see cref="OperationalStatus.Unknown" /> です。</para>
        /// </value>
        public OperationalStatus Status { get; private set; } = OperationalStatus.Unknown;

        /// <summary>
        /// IP アドレスのプレフィックスまたはネットワーク部分の、ビット単位の長さを取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="int" /> 型。
        /// <para>IP アドレスのプレフィックスまたはネットワーク部分の、ビット単位の長さ。</para>
        /// </value>
        public int PrefixLength { get; private set; }

        /// <summary>
        /// ネットワークインターフェイスアドレスを列挙します。
        /// </summary>
        /// <param name="excludeLoopback">
        /// <see cref="NetworkInterfaceType.Loopback" /> を除外する場合は <see langword="true" />。それ以外は
        /// <see langword="false" />。
        /// </param>
        /// <param name="excludeIPv6">
        /// <see cref="AddressFamily.InterNetworkV6" /> を除外する場合は <see langword="true" />。それ以外は
        /// <see langword="false" />。
        /// </param>
        /// <param name="onlyStatusUp">
        /// <see cref="OperationalStatus.Up" /> のみを抽出する場合は <see langword="true" />。それ以外は
        /// <see langword="false" />。
        /// </param>
        /// <returns><see cref="NetIpInfo" /> の列挙子。</returns>
        public static IEnumerable<NetIpInfo> GetNetIpInfos(bool excludeLoopback = false, bool excludeIPv6 = false,
            bool onlyStatusUp = false)
        {
            var query = from adapter in NetworkInterface.GetAllNetworkInterfaces()
                let macAddress = adapter.GetPhysicalAddress()
                let props = adapter.GetIPProperties()
                let ipv4Props = adapter.Supports(NetworkInterfaceComponent.IPv4) ? props.GetIPv4Properties() : null
                let ipv6Props = adapter.Supports(NetworkInterfaceComponent.IPv6) ? props.GetIPv6Properties() : null
                from uniInfo in props.UnicastAddresses
                let ipAddress = uniInfo.Address
                let interfaceIndex = ipAddress.AddressFamily switch
                {
                    AddressFamily.InterNetwork => ipv4Props?.Index,
                    AddressFamily.InterNetworkV6 => ipv6Props?.Index,
                    _ => null,
                }
                where interfaceIndex.HasValue
                select new NetIpInfo
                {
                    InterfaceIndex = interfaceIndex.Value,
                    InterfaceDescription = adapter.Description,
                    InterfaceLinkSpeed = adapter.Speed,
                    InterfaceName = adapter.Name,
                    InterfaceType = adapter.NetworkInterfaceType,
                    IpAddress = ipAddress,
                    MacAddress = macAddress,
                    Status = adapter.OperationalStatus,
                    PrefixLength = uniInfo.PrefixLength,
                };

            if (excludeLoopback)
            {
                query = query.Where(info => info.InterfaceType != NetworkInterfaceType.Loopback);
            }

            if (excludeIPv6)
            {
                query = query.Where(info => info.IpAddress.AddressFamily != AddressFamily.InterNetworkV6);
            }

            if (onlyStatusUp)
            {
                query = query.Where(info => info.Status == OperationalStatus.Up);
            }

            return query;
        }
    }
}
