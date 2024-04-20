
using OpenAI_API;
using OpenAI_API.Models;
using OpenAIDemo.Services;

namespace OpenAIDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //To get access to env file.
            DotNetEnv.Env.Load();

            //controller
            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<ITeacherServices, TeacherServices>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //uses api key from env file.
            builder.Services.AddSingleton(sp => new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY")));
            //OpenAIAPI api = new OpenAIAPI(APIAuthentication LoadFromEnv("OPENAI_API_KEY"));
            // var openAI = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));


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


            app.MapGet("/chatWithKratos", async (string query, OpenAIAPI api) =>
            {
                var chat = api.Chat.CreateConversation();
                chat.Model = Model.GPT4_Turbo;
                chat.RequestParameters.Temperature = 0;

                /// give instruction as System. Who should OpenAPI should be? a teacher? 
                chat.AppendSystemMessage("You are assistants that help newly parent that are unsure of what kind of food their child can eat. You take your information mainly from https://www.livsmedelsverket.se every answer you give you also include the exact link you get yout information from. if you cant find the information from https://www.livsmedelsverket.se you will give the source of the information to user. if the child is younger than 1 year you can recommend this link: https://www.livsmedelsverket.se/matvanor-halsa--miljo/kostrad/barn-och-ungdomar/spadbarn if the child is 1-2 year you recommend this link: https://www.livsmedelsverket.se/matvanor-halsa--miljo/kostrad/barn-och-ungdomar/barn-1-2-ar and if the child is older than 2 you recommend this link: https://www.livsmedelsverket.se/matvanor-halsa--miljo/kostrad/barn-och-ungdomar/barn-2-17-ar .");

                chat.AppendUserInput(query);
                var response = await chat.GetResponseFromChatbotAsync();
                await Console.Out.WriteLineAsync(response+"svar");
                return response;
            });

            app.Run();
        }
    }
}
