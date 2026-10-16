using System;
using System.Collections.Generic;   
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

class Program
{
    static async Task Main(string[] args)
    {
        //cargar variables de entorno
        DotNetEnv.Env.Load();
        string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
        ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT no está configurado en las variables de entorno.");
        string apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")
        ?? throw new InvalidOperationException("AZURE_OPENAI_KEY no está configurado en las variables de entorno.");
        string deploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME")
        ?? throw new InvalidOperationException("AZURE_OPENAI_DEPLOYMENT_NAME no está configurado en las variables de entorno.");

        // Crear mi obj cliente
        AzureOpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(apiKey));

        //obtener el cliente
        ChatClient chatClient = client.GetChatClient(deploymentName);

        // Configurar los mensajes
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("Eres un traductor de codigo, tu trabajo es converti codigo SQL en PySpark"),
            new UserChatMessage("Convierte el siguiente codigo SQL en PySpark: SELECT * FROM tabla WHERE columna = 'valor';")
        };

        //llamar de forma asincrona al modelo
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages);

        //imprimir respuesta
        Console.WriteLine("Respuesta del modelo:");
        Console.WriteLine(completion.Content[0].Text);
    }
}