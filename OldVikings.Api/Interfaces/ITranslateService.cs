using OldVikings.Api.DataTransferObjects.Translation;

namespace OldVikings.Api.Interfaces;

public interface ITranslateService
{
    Task<TranslationResponse> Translate(string text, string language);
}