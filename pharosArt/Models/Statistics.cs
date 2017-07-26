using PetaPoco;

namespace pharosArt.Models
{
    [TableName("Statistics")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class Statistics
    {
        public int MediaId { get; set; }

        public int Times { get; set; }

        public int Likes { get; set; }


    }
}