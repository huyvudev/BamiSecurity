using CR.RabbitMQ;
using CR.RabbitMQ.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CR.RunProcess.Worker.Csharp.Consumers
{
    internal class CodeRunnerConsumer : ConsumerService
    {
        public CodeRunnerConsumer(
            ILogger<CodeRunnerConsumer> logger,
            IOptions<RabbitMqConfig> config
        )
            : base(logger, config, "", "", string.Empty) { }

        public override Task ReceiveMessage(byte[] message)
        {
            string command = "";
            string inputString = "xin chào";

            // Tạo một quy trình để thực thi
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = @"",
                Arguments = $"{command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
            };

            // Thực thi lệnh
            using Process process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();

            using (StreamWriter writer = process.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    writer.Write(inputString);
                }
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return Task.CompletedTask;
        }
    }
}
