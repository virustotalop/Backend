using EFModeling.EntityProperties.FluentAPI.Required;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program
{
    public static void Main(string[] args)
    {
        using (var context = new BackendDbContext())
        {
            context.Database.EnsureCreated();


        }
    }
}
