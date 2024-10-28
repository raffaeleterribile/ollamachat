namespace OllamaChat {
	using Microsoft.Maui.Controls;

	public partial class AppShell : Shell {
		public AppShell() {
			InitializeComponent();
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			CenterWindow();
		}

		private void CenterWindow() {
			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
			var window = Application.Current.MainPage.Window;

			// Calcola la dimensione preferita del contenuto
			var preferredSize = CalculatePreferredSize();

			// Imposta la dimensione della finestra in base al contenuto
			window.Width = 800; // Imposta una larghezza desiderata
			window.Height = 600; // Imposta un'altezza desiderata

			// Calcola la posizione per centrare la finestra
			window.X = (mainDisplayInfo.Width / mainDisplayInfo.Density - window.Width) / 2;
			window.Y = (mainDisplayInfo.Height / mainDisplayInfo.Density - window.Height) / 2;
		}

		private Size CalculatePreferredSize() {
			// Supponiamo di avere un layout principale chiamato "mainLayout"
			var mainLayout = this.MainLayout.Content as Layout;
			if (mainLayout == null) {
				return new Size(800, 600); // Dimensione predefinita se il layout non è disponibile
			}

			// Misura il layout per ottenere la dimensione preferita
			mainLayout.Measure(double.PositiveInfinity, double.PositiveInfinity);
			var desiredSize = mainLayout.DesiredSize;

			// Aggiungi un margine per evitare che la finestra sia troppo stretta
			return new Size(desiredSize.Width + 20, desiredSize.Height + 20);
		}
	}
}
