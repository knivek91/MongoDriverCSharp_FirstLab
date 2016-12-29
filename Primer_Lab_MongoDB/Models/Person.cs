using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Person
    {

        #region Property
        [BsonId]
        [BsonElement("_id")]
        public MongoDB.Bson.ObjectId ID { get; set; }
        [BsonElement("user")]
        public string Name { get; set; }
        [BsonElement("age")]
        public string Age { get; set; }
        [BsonElement("guid")]
        public string Guid { get; set; }
        #endregion

        #region Constructor
        public Person() { }

        public Person(MongoDB.Bson.ObjectId pID, string pName, string pAge, string pGuid)
        {
            this.ID = pID;
            this.Name = pName;
            this.Age = pAge;
            this.Guid = pGuid;
        }
        #endregion

    }
}