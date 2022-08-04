// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="MareMare">
// Copyright © 2022 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net.NetworkInformation;

namespace IpAddressMonitor.WinFormsApp;

/// <summary>
/// メインフォームを表します。
/// </summary>
public partial class MainForm : Form
{
    /// <summary>動機コンテキストを表します。</summary>
    private SynchronizationContext? _syncContext;

    /// <summary>
    /// <see cref="MainForm" /> クラスの新しいインスタンスを生成します。
    /// </summary>
    public MainForm()
    {
        this.InitializeComponent();

        this.FormBorderStyle = FormBorderStyle.None;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.StartPosition = FormStartPosition.Manual;

        NetworkChange.NetworkAddressChanged += this.NetworkChange_NetworkAddressChanged;
        NetworkChange.NetworkAvailabilityChanged += this.NetworkChange_NetworkAvailabilityChanged;
    }

    /// <inheritdoc />
    public override bool AutoSize
    {
        get => true;
        set { }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.components is not null)
            {
                this.components.Dispose();
            }

            NetworkChange.NetworkAddressChanged -= this.NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged -= this.NetworkChange_NetworkAvailabilityChanged;
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (this.DesignMode)
        {
            return;
        }

        var infos = MainForm.GetAvailableNetIpv4Infos();
        this.UpdateInformationText(infos);
        this.UpdateInformationState(infos.Any());

        var target = this.labelOfInformation;

        Point? mousePressedPoint = null;
        target.MouseEnter += (_, _) => mousePressedPoint = null;
        target.MouseDown += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePressedPoint = e.Location;
            }
        };
        target.MouseMove += (_, e) =>
        {
            this.Left += e.X - mousePressedPoint?.X ?? 0;
            this.Top += e.Y - mousePressedPoint?.Y ?? 0;
        };
        target.MouseUp += (_, _) => SnapToScreenIfHasMousePressedPoint();
        target.MouseLeave += (_, _) => SnapToScreenIfHasMousePressedPoint();

        void SnapToScreenIfHasMousePressedPoint()
        {
            if (mousePressedPoint.HasValue)
            {
                this.SnapToScreen();
                mousePressedPoint = null;
            }
        }
    }

    /// <inheritdoc />
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        this._syncContext = SynchronizationContext.Current;
    }

    /// <inheritdoc />
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        if (this.DesignMode)
        {
            return;
        }

        var screen = Screen.FromPoint(this.Location);
        this.Left = screen.WorkingArea.Right - this.Width;
        this.Top = screen.WorkingArea.Top;
        this.SnapToScreen();
    }

    /// <summary>
    /// 有効な <see cref="NetIpInfo" /> のコレクションを取得します。
    /// </summary>
    /// <returns>有効な <see cref="NetIpInfo" /> のコレクション。</returns>
    private static IReadOnlyCollection<NetIpInfo> GetAvailableNetIpv4Infos()
    {
        var infos = NetIpInfo.GetNetIpInfos(
                onlyStatusUp: true,
                excludeLoopback: true,
                excludeIPv6: true)
            .OrderBy(info => new Version(info.IpAddress.ToString()))
            .ToArray();
        return infos;
    }

    /// <summary>
    /// デスクトップへスナップします。
    /// </summary>
    private void SnapToScreen()
    {
        var screen = Screen.FromPoint(this.Location);
        if (CanSnap(this.Left, screen.WorkingArea.Left))
        {
            this.Left = screen.WorkingArea.Left;
        }

        if (CanSnap(this.Top, screen.WorkingArea.Top))
        {
            this.Top = screen.WorkingArea.Top;
        }

        if (CanSnap(screen.WorkingArea.Right, this.Right))
        {
            this.Left = screen.WorkingArea.Right - this.Width;
        }

        if (CanSnap(screen.WorkingArea.Bottom, this.Bottom))
        {
            this.Top = screen.WorkingArea.Bottom - this.Height;
        }

        static bool CanSnap(int pos, int edge)
        {
            const int snapDist = 100;
            var delta = pos - edge;
            return delta is > 0 and <= snapDist;
        }
    }

    /// <summary>
    /// IP 情報を表示更新します。
    /// </summary>
    /// <param name="netIpInfos"><see cref="NetIpInfo" /> のコレクション。</param>
    private void UpdateInformationText(IEnumerable<NetIpInfo>? netIpInfos = null)
    {
        var infos = netIpInfos ?? MainForm.GetAvailableNetIpv4Infos();
        var text = string.Join(
            Environment.NewLine,
            infos.Select(info => $"{info.IpAddress}/{info.PrefixLength} {info.InterfaceType}"));
        this.labelOfInformation.Text = text;
    }

    /// <summary>
    /// ネットワーク状態を表示更新します。
    /// </summary>
    /// <param name="e"><see cref="NetworkAvailabilityEventArgs" />。</param>
    private void UpdateInformationState(NetworkAvailabilityEventArgs? e) =>
        this.UpdateInformationState(e?.IsAvailable);

    /// <summary>
    /// ネットワーク状態を表示更新します。
    /// </summary>
    /// <param name="isAvailable">
    /// ネットワークが利用可能な場合は <see langword="true" />。利用できない場合は <see langword="false" />。それ以外は
    /// <see langword="null" />。
    /// </param>
    private void UpdateInformationState(bool? isAvailable)
    {
        Color? color = isAvailable switch
        {
            true => SystemColors.Info,
            false => Color.MistyRose,
            _ => null,
        };

        this.labelOfInformation.Enabled = color.HasValue;
        this.labelOfInformation.BackColor = color ?? SystemColors.Control;
    }

    /// <summary>
    /// ネットワーク インターフェイスの IP アドレスが変更された場合に発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントのソースを表す <see cref="object" />。</param>
    /// <param name="e">イベントデータを格納している <see cref="EventArgs" />。</param>
    private void NetworkChange_NetworkAddressChanged(object? sender, EventArgs e) =>
        this._syncContext?.Post(
            _ => this.UpdateInformationText(),
            null);

    /// <summary>
    /// ネットワークの可用性に変更があった場合に発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントのソースを表す <see cref="object" />。</param>
    /// <param name="e">イベントデータを格納している <see cref="NetworkAvailabilityEventArgs" />。</param>
    private void NetworkChange_NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e) =>
        this._syncContext?.Post(
            state => this.UpdateInformationState(state as NetworkAvailabilityEventArgs),
            e);
}
