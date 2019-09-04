using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;

namespace KCore.Stored
{
    public static class Online
    {
        private static String user = R.MongoUser;
        private static string dbase = "kcore";
        private static MongoClient client = new MongoClient($"mongodb://{user}@kcore-shard-00-00-s93jd.mongodb.net:27017,kcore-shard-00-01-s93jd.mongodb.net:27017,kcore-shard-00-02-s93jd.mongodb.net:27017/test?ssl=true&replicaSet=kcore-shard-0&authSource=admin&retryWrites=true&w=majority");

        /// <summary>
        /// Save the information in the mongodb
        /// </summary>
        /// <typeparam name="T">Model base</typeparam>
        /// <param name="model"></param>
        /// <param name="table">table name, use only this a especific tables</param>
        /// <returns>-1 error, 1 added, 2 update.</returns>
        public static C.Database.Actions Save<T>(T model, string table = null) where T : KCore.Base.BaseModel
        {
            var db = client.GetDatabase(dbase);
            if (table == null)
                table = model.GetType().Name; //KCore.Shell.FormatString.JavaScriptFormat(model);
            //else
            //   table = KCore.Shell.FormatString.JavaScriptFormat(table);


            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", model.Id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                var update = Builders<T>.Update.Set("updated", result.updated + 1).CurrentDate("lastModified");
                var res = collection.UpdateOne(filter, update);
                return C.Database.Actions.Updated;
            }
            else
            {
                collection.InsertOne(model);
                return C.Database.Actions.Added;
            }


            return C.Database.Actions.Updated;
        }

        public static C.Database.Actions Save(string name, string json)
        {

            var db = client.GetDatabase(dbase);
            var collection = db.GetCollection<Colletion>(KCore.Shell.FormatString.JavaScriptFormat(name));
            var obj = new Colletion(R.Company.ID, json);

            var filter = Builders<Colletion>.Filter.Eq("_id", obj.Id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                var update = Builders<Colletion>.Update.Set("updated", result.updated + 1).CurrentDate("lastModified");
                var res = collection.UpdateOne(filter, update);
                return C.Database.Actions.Updated;
            }
            else
            {
                collection.InsertOne(obj);
                return C.Database.Actions.Added;
            }
        }




    }


    public class Colletion
    {
        public string Id { get; set; }
        public DateTime createdDate;
        public int updated;
        public string @object;
        public string customer;
        public DateTime? lastModified;

        public Colletion(string customer, string json)
        {
            @object = json;
            this.customer = customer;
            Id = KCore.Security.Hash.MD5(customer, json);
            createdDate = DateTime.Now;
        }

        public T Convert<T>()
        {
            throw new NotImplementedException();
        }
    }
}
