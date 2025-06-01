namespace bestvinnytsa.web.Data
{
    /// <summary>
    /// Налаштування для MongoDB (зчитуються з appsettings.json).
    /// </summary>
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
