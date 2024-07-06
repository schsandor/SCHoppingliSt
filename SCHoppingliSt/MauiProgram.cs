using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace SCHoppingliSt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IDataService, LiteDBService>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<ShopViewModel>();
            builder.Services.AddSingleton<ShopPage>();

            builder.Services.AddSingleton<EditShopViewModel>();
            builder.Services.AddSingleton<EditShopPage>();

            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, "trace.log");
            TextWriterTraceListener[] listeners = 
            [
                new TextWriterTraceListener(fullPath),
                new TextWriterTraceListener(Console.Out)
            ];
            Trace.Listeners.AddRange(listeners);
            Trace.AutoFlush = true;
            Trace.WriteLine("App started");
            Trace.WriteLine($"AppDataDirectory: {path}");
            return builder.Build();
        }
    }
}
