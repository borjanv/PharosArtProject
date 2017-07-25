using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


using System.Linq;
using System.Web;
using PetaPoco;

namespace pharosArt.Models
{
    [TableName("mauroTest")]
    [PrimaryKey("Id")]
    public class Statistics
    {
        public int id { get; set; }

        public int times { get; set; }
    }
}