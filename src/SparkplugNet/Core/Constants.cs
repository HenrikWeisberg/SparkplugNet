// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class that contains constant values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SparkplugNet.Core;

/// <summary>
/// A class that contains constant values.
/// </summary>
internal static class Constants
{

    // Begin HEWA: Compliance with Esclipse Sparkplug 3.0.0 standard. Must handle the node rebirth command
    /// <summary>
    /// The node control rebirth metric name.
    /// </summary>
    public const string NodeControlRebirthName = "Node Control/Rebirth";
    // End HEWA

    /// <summary>
    /// The session number metric name.
    /// </summary>
    internal const string SessionNumberMetricName = "bdSeq";

    /// <summary>
    /// The epoch.
    /// </summary>
    internal static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
}
