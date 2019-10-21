using MongoDB.Driver;

namespace KCore.Shell
{
    public static class PanicInfo
    {

        private static MongoClient client = new MongoClient("mongodb://kcore:O31x2IKRx3Zy5VUi@kcore-shard-00-00-s93jd.mongodb.net:27017,kcore-shard-00-01-s93jd.mongodb.net:27017,kcore-shard-00-02-s93jd.mongodb.net:27017/test?ssl=true&replicaSet=kcore-shard-0&authSource=admin&retryWrites=true&w=majority");

        public static void BaseException()
        {

            var a = client.GetDatabase("kcore");
            a.CreateCollection("kcore");
            var b = a.GetCollection<kcore>("base_exception");
            b.InsertOne(new kcore { id = 1, name = "Bruno" });
        }

    }


    public class kcore
    {
        public int id;
        public string name;
    }
}
