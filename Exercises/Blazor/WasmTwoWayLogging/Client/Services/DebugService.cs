using Blazor.WasmTwoWayLogging.Client.Library;
using Blazor.WasmTwoWayLogging.Client.Logging;

namespace Blazor.WasmTwoWayLogging.Client.Services
{
    public class DebugService
    {
        private readonly JsConsole _console;

        public DebugService(JsConsole console)
        {
            Logger.Log.Trace("Ctor");
            _console = console;
        }

        public async Task HandleLogFromServer(string logMessage)
        {
            await _console.LogAsync(logMessage);
        }
    }
}
