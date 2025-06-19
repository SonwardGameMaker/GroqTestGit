using GroqNet;
using GroqNet.ChatCompletions;
using System.Text;

internal class GroqService
{
    private readonly GroqClient _ai;
    private BotInstructions _instructions;

    public GroqService(string apiKey, GroqModel model, BotInstructions? botInstructions = null)
    {
        _ai = new GroqClient(apiKey, model);
        if (botInstructions == null)
        {
            _instructions = BotInstructions.DefaultInstructions;
        }
        else
            _instructions = botInstructions;
    }

    public BotInstructions Instructions { get => _instructions; }

    public async Task<string> AskAsync(string prompt)
    {
        try
        {
            var history = new GroqChatHistory { new(prompt) };
            var rsp = await _ai.GetChatCompletionsAsync(history);            
            return rsp.Choices.First().Message.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
            return "ERROR: " + ex.Message;
        }
    }

    public async Task<string> AskAsync(string prompt, UserCurrentData userData, GroqChatHistory chatHistory)
    {
        try
        {
            chatHistory.Add(new GroqMessage(GroqChatRole.System, UserDataToPromt(userData)));
            chatHistory.AddUserMessage(prompt);
            var rsp = await _ai.GetChatCompletionsAsync(chatHistory);
            var result = rsp.Choices.First().Message.Content;
            chatHistory.AddAssistantMessage(result);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
            return "ERROR: " + ex.Message;
        }
    }

    public async Task<string> TestGroqReachabilityAsync()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://api.groq.com");
            return $"Status: {response.StatusCode}";
        }
        catch (Exception ex)
        {
            return $"FAILED: {ex.Message}\nINNER: {ex.InnerException?.Message}";
        }
    }

    public async Task<string> TestGroqPostAsync(string apiKey)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var content = new StringContent(
                """
            {
              "model": "llama3-8b-8192",
              "messages": [{"role": "user", "content": "Hello"}]
            }
            """,
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            var body = await response.Content.ReadAsStringAsync();

            return $"Status: {response.StatusCode}\nBody: {body}";
        }
        catch (Exception ex)
        {
            return $"FAILED: {ex.Message}\nINNER: {ex.InnerException?.Message}";
        }
    }


    public GroqChatHistory CreateChatHistory()
        => new GroqChatHistory { _instructions.BaseInstructions };

    private string UserDataToPromt(UserCurrentData userData)
        => $"User name: {userData.UserName}" +
        $"\nPassport photo uploaded: {userData.PassportPhotoUploaded}" +
        $"\nDriver license photo uploaded: {userData.DriverLicensePhotoUploaded}" +
        $"\nPhotos confirmed: {userData.PhotosConfirmed}" +
        $"\nPrice confirmed: {userData.PriceConfirmed}";
}
