using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace HPDiff.Services;

internal class Service
{
	[PluginService] internal static IFramework Framework { get; private set; } = null!;
	[PluginService] internal static IClientState ClientState { get; private set; } = null!;
	[PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
	[PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
	[PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;
}