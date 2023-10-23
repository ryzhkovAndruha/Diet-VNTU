using System.Text;

namespace AI_Diet.Services
{
    public class ChatGptService : IChatGptService
    {
        private HttpClient _chatGptClient;
        private IConfiguration _configuration;

        private const string CHAT_POST_ADDRESS = "https://api.openai.com/v1/chat/completions";
        private const string CHAT_GPT_TOKEN = "ChatGptToken";

        public ChatGptService(IConfiguration configuration)
        {
            _configuration = configuration;

            InitHttpClient();
        }

        public string Ask(string chatRequest)
        {
            var content = new StringContent(chatRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _chatGptClient.PostAsync(CHAT_POST_ADDRESS, content).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        private void InitHttpClient()
        {
            _chatGptClient = new HttpClient();
            var token = _configuration.GetValue<string>(CHAT_GPT_TOKEN);

            _chatGptClient.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");
        }
    }
}
