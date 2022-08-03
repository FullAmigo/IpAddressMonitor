// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="MareMare">
// Copyright © 2022 MareMare. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace IpAddressMonitor.WinFormsApp;

/// <summary>
/// アプリケーションのエントリポイントを提供します。
/// </summary>
internal static class Program
{
    /// <summary>
    /// アプリケーションのメインエントリポイントです。
    /// </summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var mainForm = new MainForm();
        Application.Run(mainForm);
    }
}
