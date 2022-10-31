using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesData.Entities;
public class Room
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public RoomStatus? Status { get; set; }

    public List<User>? Users { get; set; }
    public List<Outlay>? Outlays { get; set; }
}
