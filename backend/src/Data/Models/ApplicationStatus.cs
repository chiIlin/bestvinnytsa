namespace bestvinnytsa.web.Data.Models
{
    /// <summary>
    /// Статуси заявки: 
    /// - Pending (Очікує обробки), 
    /// - Approved (Підтверджено), 
    /// - Rejected (Відхилено).
    /// </summary>
    public enum ApplicationStatus : byte
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
