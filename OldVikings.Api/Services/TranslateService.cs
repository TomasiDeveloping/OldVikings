using DeepL;
using Microsoft.Extensions.Caching.Memory;
using OldVikings.Api.DataTransferObjects.Translation;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Services;

public class TranslateService(IConfiguration configuration, IMemoryCache cache) : ITranslateService
{
    public async Task<TranslationResponse?> Translate(string text, string targetLanguage, CancellationToken cancellationToken)
    {
        var cacheKey = $"translation_{targetLanguage}_{text.GetHashCode()}";

        if (cache.TryGetValue(cacheKey, out TranslationResponse? cached))
        {
            return cached;
        }

        var translator = new Translator(configuration["DeeplAuthKey"]!);

        var preparedText = text.Replace("\n", "<br>");


        var options = new TextTranslateOptions
        {
            PreserveFormatting = true,
            SentenceSplittingMode = SentenceSplittingMode.All,
            TagHandling = "html",
        };

        var result = await translator.TranslateTextAsync(preparedText, null, targetLanguage, options, cancellationToken);

        var response = new TranslationResponse(result.Text);

        cache.Set(cacheKey, response, TimeSpan.FromHours(12));

        return response;
    }
}