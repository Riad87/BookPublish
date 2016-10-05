using System.ComponentModel.DataAnnotations;

namespace bookPublishDB
{
    public class Theme
    {
        public int ID { get; set; }
        public string ThemeName { get; set; }
        public int Active { get; set; }
           
        
    }
}