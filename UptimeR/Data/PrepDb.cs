using Microsoft.EntityFrameworkCore;
using UptimeR.Domain;
using UptimeR.Persistance;

namespace UptimeR.Data;

public static class PrepDb
{
    public static void Preppopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedDate(serviceScope.ServiceProvider.GetService<DatabaseContext>()!);
        }
    }


    private static void SeedDate(DatabaseContext context)
    {
        var dbexist = DatabaseExist(context);

        if(!dbexist)
        {
            Console.WriteLine("--> Attempting to apply migrations!");
            try
            {
                context.Database.Migrate();
            }
            catch(Exception)
            {
            }
        }

        if(!context.URLs.Any())
        {
            Console.WriteLine("--> Seeding Sample Data!");

            context.AddRange(
                new URL() { ServiceName = "Loopback address", Url = "127.0.0.1", OnlyPing = true, Interval = 1 },
                new URL() { ServiceName = "Google", Url = "https://google.com", OnlyPing = false, Interval = 10 }
                );

            context.SaveChanges();
        }
    }

    private static bool DatabaseExist(DatabaseContext context)
    {
        try
        {
            context.Set<URL>().Count();
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}
