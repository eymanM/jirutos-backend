var builder = WebApplication.CreateBuilder(args);

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
    cfg.CreateMap<WorklogForIssue, WorklogForJiraIssueDto>();
});
IMapper mapper = configuration.CreateMapper();

builder.Services.AddSingleton<IMapper>(mapper);

var app = builder.Build();

app.UseCors("MyPolicy");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();