using GroqNet;
using GroqNet.ChatCompletions;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "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";

        GroqService groqService = new GroqService(apiKey, GroqModel.LLaMA3_70b);
        GroqChatHistory groqMessages = new GroqChatHistory { BotInstructions.DefaultInstructions.BaseInstructions };
        UserCurrentData userCurrentData = new UserCurrentData("Roman");

        //Console.WriteLine($"Groq reach: {await groqService.TestGroqReachabilityAsync() }");
        Console.WriteLine($"Groq POST: {await groqService.TestGroqPostAsync(apiKey)}");

        //Console.WriteLine("Without history //----------------------------------------------------------------------");
        //string message1 = "Hello";
        //Console.WriteLine($"User prompt: {message1}");
        //var response1 = await groqService.AskAsync(message1);
        //Console.WriteLine("Groq response:");
        //Console.WriteLine(response1);

        //Console.WriteLine("With history //----------------------------------------------------------------------");
        //string message2 = "Hello, did I Confirmed my passport?";
        //Console.WriteLine($"User prompt: {message2}");
        //var response2 = await groqService.AskAsync(message2, userCurrentData, groqMessages);
        //Console.WriteLine("Groq response:");
        //Console.WriteLine(response2);
    }
}
