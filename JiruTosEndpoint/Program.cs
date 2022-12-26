using Foundation.Interfaces;
using JiruTosEndpoint.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Audience = builder.Configuration["AWS:AppClientId"];
        options.Authority = "https://cognito-idp.eu-west-1.amazonaws.com/eu-west-1_iBrmbuJSm";
    });

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

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton(configuration.CreateMapper());
builder.Services.AddSingleton<IDatabase>(new Database(builder.Configuration["AppData:MongoConnStr"]));

var app = builder.Build();

app.UseCors("MyPolicy");

app.UseCookiePolicy();

app.UseExceptionHandler("/Error");

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
{
    context.ProtocolMessage.Scope = "openid";
    context.ProtocolMessage.ResponseType = "code";

    var cognitoDomain = builder.Configuration["Authentication:Cognito:CognitoDomain"];

    var clientId = builder.Configuration["Authentication:Cognito:ClientId"];
    
    var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{builder.Configuration["Authentication:Cognito:AppSignOutUrl"]}";

    context.ProtocolMessage.IssuerAddress = $"{cognitoDomain}/logout?client_id={clientId}&logout_uri={logoutUrl}&redirect_uri={logoutUrl}";

    // delete cookies
    context.Properties.Items.Remove(CookieAuthenticationDefaults.AuthenticationScheme);
    // close openid session
    context.Properties.Items.Remove(OpenIdConnectDefaults.AuthenticationScheme);

    return Task.CompletedTask;
}
