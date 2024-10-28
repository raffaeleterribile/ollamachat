using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OllamaChat.Core;
using System.Reflection;

namespace OllamaChat {
	public static class MauiProgram {
		public static IServiceProvider ServiceProvider { get; private set; }

		public static MauiApp CreateMauiApp() {
			var builder = MauiApp.CreateBuilder();

			// Aggiungi la configurazione dal file appsettings.json
			var assembly = Assembly.GetExecutingAssembly();
			using var stream = assembly.GetManifestResourceStream("OllamaChat.appsettings.json");
			var configuration = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();

			builder.Configuration.AddConfiguration(configuration);


			var services = new ServiceCollection();
			IConfigurationSection configurationSection = configuration.GetSection("AppConfiguration");
			services.AddSingleton<IConfigurationSection>(configurationSection);
			services.AddSingleton<ChatService>();

			ServiceProvider = services.BuildServiceProvider();

			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
