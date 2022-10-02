using Amazon;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Foundation.Interfaces;
using Foundation.Models.Structs;
using Microsoft.AspNetCore.Authorization;
using JiraService;
using System.Drawing;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : Controller
{
    private readonly IDatabase _db;
    private readonly ConfigurationManager _config;
    private readonly ILogger<JiraIssueRepository> _logger;
    private readonly AmazonCognitoIdentityProviderClient _cognito;

    public UserController(IDatabase db, ConfigurationManager config, ILogger<JiraIssueRepository> logger)
    {
        _db = db;
        _config = config;
        _logger = logger;
        _cognito = new(_config["AWS:AccessId"], _config["AWS:SecretId"], RegionEndpoint.EUWest1);
    }

    [HttpGet("{email}")]
    public ActionResult Integrations(string email)
    {
        _logger.LogWarning(User.Identity.ToString());
        var user = _db.FindUser(email);
        var integrations = user.Integrations;
        var basicIntegrationsData = integrations.Select(integ => new { type = integ.Type, name = integ.Name });
        return Ok(basicIntegrationsData);
    }

    [HttpPost]
    public async Task<ActionResult> SignUp([FromBody] EmailPasswordStruct registerUserObj)
    {
        var request = new SignUpRequest
        {
            ClientId = _config["AWS:AppClientId"],
            Password = registerUserObj.Password,
            Username = registerUserObj.Email
        };

        var emailAttribute = new AttributeType
        {
            Name = "email",
            Value = registerUserObj.Email
        };
        request.UserAttributes.Add(emailAttribute);

        try
        {
            var response = await _cognito.SignUpAsync(request);
            return Ok(response);
        } 
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Problem(e.Message);
        }
    }

    [HttpGet("{email}/{code}")]
    public async Task<ActionResult> ConfirmUserByCode(string email, string code)
    {
        ConfirmSignUpRequest request = new()
        {
            ClientId = _config["AWS:AppClientId"],
            ConfirmationCode = code,
            Username = email
        };

        try
        {
            var response = await _cognito.ConfirmSignUpAsync(request);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Problem(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> SignIn([FromBody] EmailPasswordStruct user)
    {
        var request = new AdminInitiateAuthRequest
        {
            UserPoolId = _config["AWS:UserPoolId"],
            ClientId = _config["AWS:AppClientId"],
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
        };

        request.AuthParameters.Add("USERNAME", user.Email);
        request.AuthParameters.Add("PASSWORD", user.Password);

        try
        {
            var response = await _cognito.AdminInitiateAuthAsync(request);
            return Ok(response.AuthenticationResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Problem(e.Message);
        }
    }

    [HttpGet("{username}/{refreshToken}")]
    public async Task<ActionResult> RenewTokens(string username, string refreshToken)
    {
        AmazonCognitoIdentityProviderClient provider =new (new AnonymousAWSCredentials());
        CognitoUserPool userPool = new (_config["AWS:UserPoolId"], _config["AWS:AppClientId"], provider);
        CognitoUser user = new (username, _config["AWS:AppClientId"], userPool, provider);

        user.SessionTokens = new CognitoUserSession(null, null, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(3600));
        var request = new InitiateRefreshTokenAuthRequest() { AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH };
        try
        {
            var response = await user.StartWithRefreshTokenAuthAsync(request);
            return Ok(response.AuthenticationResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Problem(e.Message);
        }
    }
}

