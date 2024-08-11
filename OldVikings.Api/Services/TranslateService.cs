using DeepL;
using OldVikings.Api.DataTransferObjects.Translation;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Services;

public class TranslateService(IConfiguration configuration) : ITranslateService
{
    public async Task<TranslationResponse> Translate(string text, string language)
    {
        var translator = new Translator(configuration["DeeplAuthKey"]!);

        var translatedText = await translator.TranslateTextAsync(text, null, language);

        return new TranslationResponse(translatedText.Text);
    }
}