using System.ComponentModel.DataAnnotations;

namespace bookPublishDB
{
    public class AccountType
    {
        public int ID { get; set; }
        public string Details { get; set; }

        [Required]
        public Partner Partner { get; set; }
    }
}