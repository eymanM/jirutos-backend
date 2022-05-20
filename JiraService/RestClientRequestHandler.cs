namespace JiraService;

public class RestClientRequestHandler
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;
    private readonly RestRequest _request = new();

    public RestClientRequestHandler(IConfiguration config)
    {
        _config = config;
        _client = new(_config["AppData:URL"] + @"/search")
        {
            Authenticator = new HttpBasicAuthenticator(_config["AppData:Email"], _config["AppData:Token"])
        };
    }

    public RestResponse GetJQLResponse(BodyJQLModel body)
    {
        _request.AddJsonBody(body);

        return _client.PostAsync(_request).GetAwaiter().GetResult();
    }
}