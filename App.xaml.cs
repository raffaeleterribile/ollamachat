using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OllamaChat.Core;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OllamaChat {
	/// <summary>
	/// Fornisci un comportamento specifico dell'applicazione in supplemento alla classe Application predefinita.
	/// </summary>
	sealed partial class App : Application {
		public static IServiceProvider ServiceProvider { get; private set; }

		/// <summary>
		/// Inizializza l'oggetto Application singleton. Si tratta della prima riga del codice creato
		/// creato e, come tale, corrisponde all'equivalente logico di main() o WinMain().
		/// </summary>
		public App() {
			this.InitializeComponent();
			this.Suspending += OnSuspending;

			var configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var services = new ServiceCollection();
			IConfigurationSection configurationSection = configuration.GetSection("AppConfiguration");
			services.AddSingleton<IConfigurationSection>(configurationSection);
			services.AddSingleton<ChatService>();

			ServiceProvider = services.BuildServiceProvider();
		}

		/// <summary>
		/// Richiamato quando l'applicazione viene avviata normalmente dall'utente finale. All'avvio dell'applicazione
		/// verranno usati altri punti di ingresso per aprire un file specifico.
		/// </summary>
		/// <param name="e">Dettagli sulla richiesta e sul processo di avvio.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e) {
			Frame rootFrame = Window.Current.Content as Frame;

			// Non ripetere l'inizializzazione dell'applicazione se la finestra già dispone di contenuto,
			// assicurarsi solo che la finestra sia attiva
			if (rootFrame == null) {
				// Creare un frame che agisca da contesto di navigazione e passare alla prima pagina
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
					//TODO: caricare lo stato dall'applicazione sospesa in precedenza
				}

				// Posizionare il frame nella finestra corrente
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false) {
				if (rootFrame.Content == null) {
					// Quando lo stack di esplorazione non viene ripristinato, passare alla prima pagina
					// configurando la nuova pagina per passare le informazioni richieste come parametro di
					// navigazione
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}

				// Imposta la dimensione della finestra
				ApplicationView.PreferredLaunchViewSize = new Size(400, 400);
				ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

				// Centra la finestra
				var displayInformation = DisplayInformation.GetForCurrentView();
				var screenWidth = displayInformation.ScreenWidthInRawPixels;
				var screenHeight = displayInformation.ScreenHeightInRawPixels;

				var windowWidth = ApplicationView.PreferredLaunchViewSize.Width;
				var windowHeight = ApplicationView.PreferredLaunchViewSize.Height;

				var x = (screenWidth - windowWidth) / 2;
				var y = (screenHeight - windowHeight) / 2;

				ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(windowWidth, windowHeight));
				ApplicationView.GetForCurrentView().TryResizeView(new Size(windowWidth, windowHeight));

				// Assicurarsi che la finestra corrente sia attiva
				Window.Current.Activate();
			}
		}

		/// <summary>
		/// Chiamato quando la navigazione a una determinata pagina ha esito negativo
		/// </summary>
		/// <param name="sender">Frame la cui navigazione non è riuscita</param>
		/// <param name="e">Dettagli sull'errore di navigazione.</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Richiamato quando l'esecuzione dell'applicazione viene sospesa. Lo stato dell'applicazione viene salvato
		/// senza che sia noto se l'applicazione verrà terminata o ripresa con il contenuto
		/// della memoria ancora integro.
		/// </summary>
		/// <param name="sender">Origine della richiesta di sospensione.</param>
		/// <param name="e">Dettagli relativi alla richiesta di sospensione.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e) {
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: salvare lo stato dell'applicazione e arrestare eventuali attività eseguite in background
			deferral.Complete();
		}
	}
}
