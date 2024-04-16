namespace OpenAIDemo.Services
{
    public interface ITeacherServices
    {
        Task<string[]> GetAllTeachers();
        Task<string> GetTeacherByName(string name);
    }
}
