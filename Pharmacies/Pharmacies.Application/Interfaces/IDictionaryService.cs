namespace Pharmacies.Application.Interfaces;

public interface IDictionaryService
{
    Task Add(string dictionaryType, string name);
    Task Delete(string dictionaryType, int id);
}