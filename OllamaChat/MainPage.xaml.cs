using OllamaChat.Core;

namespace OllamaChat {
	public partial class MainPage : ContentPage {
		private readonly ChatService chatService;
		int count = 0;

		public MainPage() {
			InitializeComponent();
			chatService = MauiProgram.ServiceProvider.GetService<ChatService>()!;
		}

		async private void Send_Clicked(object sender, EventArgs e) {
			Output.Text = await chatService.SendMessageAsync(Input.Text);
		}
	}

}
