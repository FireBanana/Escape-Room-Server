using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

public static class Database
{
    public static List<DatabaseItem> GetResults()
    {
        using (var database = new LiteDatabase("Main"))
        {
            var collection = database.GetCollection<DatabaseItem>();
            var iter = collection.FindAll();

            var list = new List<DatabaseItem>();

            foreach(var item in iter)
            {
                list.Add(item);
            }

            return list;
        }
    }

    public static void AddEntry(string teamName, string score, string time)
    {
        using (var database = new LiteDatabase("Main"))
        {
            var item = new DatabaseItem() { TeamName = teamName, Score = score, Time = time };
            var collection = database.GetCollection<DatabaseItem>();
            collection.Insert(item);
        }
    }
}

public class DatabaseItem
{
    public int Id { get; set; }
    public string TeamName { get; set; }
    public string Score { get; set; }
    public string Time { get; set; }
}

