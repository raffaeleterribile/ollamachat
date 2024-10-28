using Microsoft.Extensions.Configuration;
using OllamaSharp;
using OllamaSharp.Models.Chat;

namespace OllamaChat.Core {
	internal class ChatService {
		private const string MODEL_ID = "phi3:mini";

		private readonly IConfigurationSection config;
		private readonly string serverUrl;
		private readonly IOllamaApiClient client;
		private readonly List<Message> chatHistory;

		public ChatService(IConfigurationSection config) {
			this.config = config;
			this.serverUrl = this.config["ServerUrl"];
			this.client = new OllamaApiClient(this.serverUrl);
			this.chatHistory = new List<Message>() {
								new Message {
									Role = ChatRole.System,
									Content = "Ciao! Come posso aiutarti?"
								}
							};
		}

		internal async Task<string> SendMessageAsync(string message) {

			chatHistory.Add(new Message {
				Role = ChatRole.User,
				Content = message
			});

			var request = new ChatRequest {
				Model = MODEL_ID,
				Stream = false,
				Messages = chatHistory
			};

			var response = await client.Chat(request).StreamToEnd();

			chatHistory.Add(new Message {
				Role = ChatRole.Assistant,
				Content = message
			});

			return response.Message.Content;
		}

		internal async IAsyncEnumerable<string> SendMessageStreamAsync(string message) {

			chatHistory.Add(new Message {
				Role = ChatRole.User,
				Content = message
			});

			var request = new ChatRequest {
				Model = MODEL_ID,
				Stream = true,
				Messages = chatHistory
			};

			await foreach (var response in client.Chat(request)) {
				yield return response.Message.Content;
			}

			chatHistory.Add(new Message {
				Role = ChatRole.Assistant,
				Content = message
			});
		}
	}
}
