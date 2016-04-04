using System.ComponentModel.DataAnnotations;

namespace MnCalculator.Models
{
    public class ChildSupportForm
    {
        public ChildSupportForm()
        {
            NumberOfChildren = 1;
            ParentAOvernights = (decimal) 182.5;
        }

        [Display(Name ="Parent A Name")]
        public string ParentA { get; set; }

        [Display(Name = "Parent B Name")]
        public string ParentB { get; set; }

        [Display(Name = "IV-D Case Number")]
        public string CaseNumber { get; set; }

        [Display(Name = "Court File Number")]
        public string CourtFileNumber { get; set; }

        [Display(Name = "Enter the Number of Children")]
        [Required(ErrorMessage = "Number of Children is Required")]
        [Range(typeof(int), "1", "6", ErrorMessage = "Number of Children must be greater than 0 and less 7.  Enter 6 if you have 6 or more children.")]
        public int NumberOfChildren { get; set; }

        [Display(Name = "Gross Income")]
        [Required(ErrorMessage = "Parent A Gross Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage="Gross income must be 0 or greater.  Enter zero for no income.")]
        public decimal ParentAGrossIncome { get; set; }

        [Display(Name = "Gross Income")]
        [Required(ErrorMessage = "Parent B Gross Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Gross income must be 0 or greater.  Enter zero for no income.")]
        public decimal ParentBGrossIncome { get; set; }

        [Display(Name = "Number of overnights")]
        [Required(ErrorMessage = "Parent A overnights is required")]
        [Range(typeof(decimal), "0", "182.5", ErrorMessage = "Parent A is the parent with the least number of overnights.  Value must be between 0 and 182.5")]
        public decimal ParentAOvernights { get; set; }

        [Display(Name = "Number of overnights")]
        [Range(typeof(decimal), "0", "365", ErrorMessage = "Number of overnights must be between 0 and 365.")]
        public decimal ParentBOvernights { get; set; }

    }
}