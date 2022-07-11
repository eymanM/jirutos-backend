using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickUpService.ClickUpModels;

public class Status
{
    [JsonProperty("status")]
    public string StatusName { get; set; }

    public string Color { get; set; }
    public string Type { get; set; }
    public int Orderindex { get; set; }
}