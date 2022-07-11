using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickUpService.ClickUpModels;

public class Task
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Status Status { get; set; }
}