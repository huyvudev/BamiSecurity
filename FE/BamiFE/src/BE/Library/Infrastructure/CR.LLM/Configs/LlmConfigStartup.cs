using CR.LLM.Abstracts;
using CR.LLM.Implements;
using CR.LLM.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CR.LLM.Configs
{
    public static class LlmConfigStartup
    {
        public static void ConfigureLLM(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<LlmConfig>(
                builder.Configuration.GetSection("LLM")
            );
            builder.Services.AddSingleton<ILlmLocalization, LlmLocalization>();
            builder.Services.AddSingleton<ILlmMapErrorCode, LlmMapErrorCode>();
            builder.Services.AddSingleton<ILlmService, ChatGptService>();
        }
    }
}
