using System.ComponentModel.DataAnnotations;

namespace MnCalculator.Models
{
    public class ChildSupportForm
    {
        [Display(Name ="Parent A Gross Income")]
        public string ParentA { get; set; }

        [Display(Name = "Parent B Gross Income")]
        public string ParentB { get; set; }

        [Display(Name = "Enter the Number of Children")]
        [Required(ErrorMessage = "Number of Children is Required")]
        [Range(typeof(int), "1", "6", ErrorMessage = "Number of Children must be greater than 0 and less 7.  Enter 6 if you have 6 or more children.")]
        public int NumberOfChildren { get; set; }

        [Display(Name = "Parent A Gross Income")]
        [Required(ErrorMessage = "Parent A Gross Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage="Gross income must be 0 or greater.  Enter zero for no income.")]
        public decimal ParentAGrossIncome { get; set; }

        [Display(Name = "Parent B Gross Income")]
        [Required(ErrorMessage = "Parent B Gross Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Gross income must be 0 or greater.  Enter zero for no income.")]
        public decimal ParentBGrossIncome { get; set; }

    }
}