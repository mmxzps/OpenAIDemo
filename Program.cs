
using OpenAIDemo.Services;

namespace OpenAIDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //controller
            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<ITeacherServices, TeacherServices>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            //controlelrs
            app.MapControllers();

            app.MapGet("/getTeachers", async (ITeacherServices teacherServices) =>
            {
                var result = await teacherServices.GetAllTeachers();
                return Results.Ok(result);
            });

            app.MapGet("/getTeacherByName/{name}", async (string name, ITeacherServices teacherServices) =>
            {
                if(string.IsNullOrWhiteSpace(name))
                {
                   return Results.BadRequest("Must enter a valid name");
                }
                var result = await teacherServices.GetTeacherByName(name);
               if(result == null)
                {
                    return Results.NotFound("Couldnt find data");
                }
                return Results.Ok(result);
            });
            app.Run();
        }
    }
}
