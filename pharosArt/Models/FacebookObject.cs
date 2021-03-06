﻿using System.Runtime.Serialization;

namespace pharosArt.Models
{
    [DataContract]
    public class FacebookObject
    {
        [DataMember(Name = "birthday")]
        public string Birthday { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}