using System.Runtime.CompilerServices;
using Microsoft.JSInterop;

namespace DbChart.Web;

public static class JsInteropHelper
{
    private static readonly string? AssemblyName = typeof(Program).Assembly.GetName().Name;
    
    public static async Task<IJSObjectReference> LoadModuleAsync(this IJSRuntime js, [CallerFilePath] string path = "")
    {
        // todo: span?
        path = path.Split(AssemblyName)[1].Replace('\\', '/') + ".js";
        return await js.InvokeAsync<IJSObjectReference>("import", path);
    }
}