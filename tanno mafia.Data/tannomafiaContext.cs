using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tanno_mafia.Core.Domain;

namespace tanno_mafia.Data
{
    public class tannomafiaContext :DbContext
    {
        public DbSet<Character> tannos {  get; set; }
    }
}
