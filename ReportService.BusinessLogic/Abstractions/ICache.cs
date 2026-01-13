namespace ReportService.BusinessLogic.Abstractions;

/// <summary>
/// Интерфейс для кэширования данных.
/// </summary>
public interface ICache
{
    /// <summary>
    /// Получает значение из кэша по ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <returns>Значение из кэша или default, если не найдено.</returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// Сохраняет значение в кэш с указанным ключом и временем жизни.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <param name="value">Значение для сохранения.</param>
    /// <param name="expiration">Время жизни записи в кэше.</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

    /// <summary>
    /// Удаляет значение из кэша по ключу.
    /// </summary>
    /// <param name="key">Ключ кэша.</param>
    Task RemoveAsync(string key);
}
