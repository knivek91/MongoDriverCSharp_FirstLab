﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Connection
    {

        #region Varible
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        #endregion

        #region Constructor
        public Connection()
        {
            //var server = new MongoServerAddress("ds151028.mlab.com", 51028);
            //var credential = MongoCredential.CreateMongoCRCredential("testmongodb", "CaloMendez10", "creativecalo10");
            
            //_client = new MongoClient(new MongoClientSettings() {

            //    Credentials = new[] { credential }
            //    , Server = server
            //    , ConnectTimeout = new TimeSpan(0, 1, 0)

            //});
            //_database = _client.GetDatabase(System.Configuration.ConfigurationManager.AppSettings["mongoDataBase"]);
            try
            {
                
                string mongDB = System.Configuration.ConfigurationManager.AppSettings["mongoDBConn"];
                _client = new MongoClient(mongDB);

                var a = _client.GetDatabase("testmongodb").GetCollection<BsonDocument>("documents").AsQueryable()
                                                          .Select(x => x).ToList<BsonDocument>();
                
                //var server = new MongoServerAddress("ds151028.mlab.com", 51028);

                //var credential = MongoCredential.CreateMongoCRCredential("testmongodb", "KevinUserDB", "creativecalo10");`

                //_a = new MongoClient(new MongoClientSettings()
                //{
                //    Credentials = new[] { credential },
                //    Server = server,
                //    ConnectTimeout = TimeSpan.FromSeconds(60),
                //    SocketTimeout = TimeSpan.FromSeconds(60), 
                ////    ReadPreference = ReadPreference.Primary     
                //});

                //IMongoDatabase _db = _a.GetDatabase("testmongodb");
                //IMongoCollection<Person> collection = _db.GetCollection<Person>("documents");
                //List<Person> _lst = collection.AsQueryable().Select(x => x).ToList<Person>();
               
            }
            catch (Exception ex)
            {
                throw;
            }



        }
        #endregion

        #region Read Docs With no Model

        public List<object> getDocsWithNoModel()
        {
            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return null;

            IMongoCollection<BsonDocument> collection = null;
            collection = _database.GetCollection<BsonDocument>("documents");

            var cursor = collection.AsQueryable()
                         .Select( x => x);

            List<object> _lst = new List<object>();
            foreach (var item in cursor)
            {
                BsonDocument doc = item;
                BsonValue id =  doc.GetValue("_id");
                BsonValue age =  doc.GetValue("age");
                BsonValue user =  doc.GetValue("user");
                BsonValue guid =  doc.GetValue("guid");
                _lst.Add(new
                {
                    ID = id.AsObjectId,
                    User = user.AsString,
                    Age= age.AsString,
                    MyGuid = guid.AsString
                });
            }

            return _lst;
        }

        #endregion

        #region Read Docs With Model

        public List<Person> getDocsWithModel()
        {

            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return null;
            IMongoCollection<Person> collection = _database.GetCollection<Person>("documents");
            List<Person> _lst = collection.AsQueryable().Select(x => x).ToList<Person>();
            return _lst;
        }

        #endregion

        #region Add
        public string addDoc(Person pPerson)
        {
            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return "Cannot connect with the database.";

            string validateResult = this.validate(pPerson);
            if (!validateResult.Equals(""))
                return validateResult;

            // this is when the IMongoCollection has a BsonDocument type
            //Dictionary<string, object> _Dictionary = pPerson.GetType()
            //.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            // .ToDictionary(prop => prop.Name, prop => prop.GetValue(pPerson, null));

            //BsonDocument doc = new BsonDocument(_Dictionary);

            IMongoCollection<Person> collection = _database.GetCollection<Person>("documents");
            collection.InsertOne(pPerson);

            return "";
        }
        #endregion

        #region Modify
        public string modifyDoc(Person pPerson)
        {
            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return "Cannot connect with the database.";

            string validateResult = this.validate(pPerson, true);
            if (!validateResult.Equals(""))
                return validateResult;

            IMongoCollection<Person> collection = _database.GetCollection<Person>("documents");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("guid", pPerson.Guid);
            UpdateDefinition<Person> update = Builders<Person>.Update
                .Set("user", pPerson.Name)
                .Set("age",pPerson.Age);
            var result = collection.UpdateOne(filter, update);

            if (!result.IsModifiedCountAvailable || !result.IsAcknowledged)
                return "There was a problem updating";

            if(result.IsAcknowledged)
                if (result.ModifiedCount == 0)
                    return "Don't update any information.";

            return "";
        }
        #endregion

        #region Delete
        public string deleteDoc(string pGuid)
        {
            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return "Cannot connect with the database.";

            IMongoCollection<Person> collection = _database.GetCollection<Person>("documents");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("guid", pGuid);
            DeleteResult result = collection.DeleteOne(filter);

            if(!result.IsAcknowledged)
                return "There was a problem deleting";

            if (result.DeletedCount == 0)
                return "Don't delete any information.";

            return "";
        }        
        #endregion

        #region Find
        public Person findDoc(string pGuid)
        {
            if (_database.Client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
                return null;

            IMongoCollection<Person> collection = _database.GetCollection<Person>("documents");
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq("guid", pGuid);
            List<Person> result = collection.Find(filter).ToList<Person>();

            return result[0];
        }
        #endregion

        #region Validate
        private string validate(Person pPerson, bool isUpdate = false)
        {
            if (pPerson.Name.Equals(""))
                return "Please fill the name.";

            if (pPerson.Age == "0")
                return "Please fill the age.";

            if (isUpdate)
            {
                if (pPerson.Guid.Equals(""))
                    return "There's no Id to update.";
            }

            return "";
        }
        #endregion

    }
}