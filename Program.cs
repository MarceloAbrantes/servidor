using System;
using System.IO.Pipes;
using System.Text;

namespace PipeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Servidor esperando por conexão...");

            // Criando o servidor de pipe nomeado
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut))
            {
                // Aguardando o cliente se conectar
                pipeServer.WaitForConnection();
                Console.WriteLine("Cliente conectado.");

                // Loop infinito para comunicação contínua
                while (true)
                {
                    try
                    {
                        // Buffer para receber a mensagem do cliente
                        byte[] buffer = new byte[256];
                        int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);
                        string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine("Mensagem recebida do cliente: " + clientMessage);

                        // Respondendo com uma mensagem diferente
                        string serverMessage = $"Respondendo a essa mensagem '{clientMessage}'";
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(serverMessage);
                        pipeServer.Write(responseBuffer, 0, responseBuffer.Length);
                        pipeServer.Flush(); // Garante que a resposta seja enviada imediatamente
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
                        break;
                    }
                }
            }
        }
    }
}

