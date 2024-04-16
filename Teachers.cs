namespace OpenAIDemo
{
    public class Teachers
    {
        public static async Task<string[]> GetTeacherData()
        {
            var data = new string[]{
                "Aldor", "Christoffer", "Arnar"
            };
            return data;
        }
    }
}
