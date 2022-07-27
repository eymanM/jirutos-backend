using Foundation.Interfaces;
using JiruTosEndpoint.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<WorklogForJiraIssue, IssueWorklogDto>()
    .ForMember(x => x.CommentText, y => y
    .MapFrom(z => z.Comment.CommentContentObject
                  .FirstOrDefault()!.Content.FirstOrDefault()!.Text));
});

builder.Services.AddSingleton<IMapper>(configuration.CreateMapper());
builder.Services.AddSingleton<IDatabase>(new Database(builder.Configuration["AppData:MongoConnStr"]));

var app = builder.Build();

app.UseCors("MyPolicy");

app.UseExceptionHandler("/Error");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();