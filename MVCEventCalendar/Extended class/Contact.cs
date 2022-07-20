using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Extended_class
{
    [MetadataType(typeof(ContactMetaData))]
    public partial class Contact
    {
        public string Predmet { get; set; }
        public string Prepodavatel { get; set; }
    }

    public class ContactMetaData
    {
        //[Required(ErrorMessage ="Aud required", AllowEmptyStrings =false)]
        //[Display(Name ="Aud")]
        //public int Aud { get; set; }

        [Required(ErrorMessage = "Predmet required")]
        [Display(Name = "Predmet")]
        public int PredmetId { get; set; }

        [Required(ErrorMessage = "Prepod required")]
        [Display(Name = "Prepod")]
        public int PrepodId { get; set; }
    }
}