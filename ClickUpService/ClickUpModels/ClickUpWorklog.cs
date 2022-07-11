using System;

namespace ClickUpService.ClickUpModels;

public class ClickUpWorklog
{
    public string Id { get; set; }
    public Task Task { get; set; }
    public User User { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
    public long Duration { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public string At { get; set; }
    public TaskLocation Task_location { get; set; }
}