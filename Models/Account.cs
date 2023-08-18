using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIAssignment.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[JsonIgnore]
        public Guid ID { get; set; }
        public string firstname {  get; set; }
     public string lastname { get; set; }
     public string email {get; set; }
     public string telephone { get; set; }
     public string identitynumber { get; set; }
    }
}
