using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGentlemanLibrary.Infrastructure.Data
{
    public class ConnectionStrings
    {
        public required string DefaultConnection { get; set; }
        public required string SecondConnection { get; set; }
    }
}
