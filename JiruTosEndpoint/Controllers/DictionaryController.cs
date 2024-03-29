﻿using ClickUpService;
using Foundation;
using Foundation.Interfaces;
using JiraService;
using Microsoft.AspNetCore.Authorization;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DictionaryController : Controller
{
    private readonly DictionaryFascade _repo;
    private readonly IDatabase _db;

    public DictionaryController(IDatabase db)
    {
        _repo = new DictionaryFascade(new List<IDictionaryRepository>() {
            new JiraDictionaryRepository(), new ClickUpDictionaryRepository() });
        _db = db;
    }
    [Authorize]
    [HttpGet("{type}/{name}")]
    public ActionResult AvailableProjectsForUser(string type, string name)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var projects = _repo.AvailableProjectsForUser(_db.FindUser(email), type, name);
        return Ok(projects);
    }

    [Authorize]
    [HttpGet("{type}/{name}")]
    public ActionResult Statuses(string type, string name)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var statuses = _repo.AllStatuses(_db.FindUser(email), type, name);
        return Ok(statuses);
    }
}