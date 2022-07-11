using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickUpService.ClickUpModels;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Color { get; set; }
    public string Initials { get; set; }
    public object ProfilePicture { get; set; }
}