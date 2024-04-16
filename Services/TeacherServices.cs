
namespace OpenAIDemo.Services
{
    public class TeacherServices : ITeacherServices
    {
        public async Task<string[]> GetAllTeachers()
        {
            var teachers = await Teachers.GetTeacherData();
            return teachers;
        }

        public async Task<string> GetTeacherByName(string name)
        {
            var teachers = await Teachers.GetTeacherData();
            // var filtered = teachers.FirstOrDefault(x=>x.ToLower() == name.ToLower());
            var filtered = teachers.FirstOrDefault(x=>x.ToLower().Equals(name.ToLower()));

            if (string.IsNullOrEmpty(filtered))
            {
                return null;
            }

            return filtered;
        }
    }
}
