using System.Collections;
using System.ComponentModel.DataAnnotations;
namespace MnCalculator.Models
{
    public class ChildSupportForm : IEnumerable
    {
        public ChildSupportForm()
        {
            NumberOfChildren = 1;
            ParentAMonthlyIncomeReceived = 0;
            ParentAOvernights = 182.5;
            ParentBMonthlyIncomeReceived = 0;
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

        [Display(Name = "What is the monthly income received?")]
        [Required(ErrorMessage = "Parent A Monthly Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage="Monthly income must be 0 or greater.  Enter 0 for no income.")]
        public decimal ParentAMonthlyIncomeReceived { get; set; }

        [Display(Name = "What is the monthly income received?")]
        [Required(ErrorMessage = "Parent B Monthly Income is Required")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Monthly income must be 0 or greater.  Enter 0 for no income.")]
        public decimal ParentBMonthlyIncomeReceived { get; set; }

        [Display(Name = "What is the number of the overnights for each parent?")]
        [Required(ErrorMessage = "Parent A overnights is required")]
        [Range(typeof(decimal), "0", "182.5", ErrorMessage = "Parent A is the parent with the least number of overnights.  Value must be between 0 and 182.5")]
        public double ParentAOvernights { get; set; }

        [Display(Name = "What is the number of the overnights for each parent?")]
        [Range(typeof(decimal), "0", "365", ErrorMessage = "Number of overnights must be between 0 and 365.")]
        public decimal ParentBOvernights { get; set; }

        [Display(Name = "What is the potential income for each parent, if any?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentAPotentialIncome { get; set; }

        [Display(Name = "What is the potential income for each parent, if any?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentBPotentialIncome { get; set; }

        [Display(Name = "What is the monthly amount the joint child(ren) receive in benefits from Social Security or the U.S. Department of Veterans Affairs (VA) due to a parent's eligibility?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentASocialSecurityBenefits { get; set; }

        [Display(Name = "What is the monthly amount the joint child(ren) receive in benefits from Social Security or the U.S. Department of Veterans Affairs (VA) due to a parent's eligibility?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentBSocialSecurityBenefits { get; set; }

        [Display(Name = "If the joint child(ren) receive Social Security or VA benefits due to Parent A's eligibility, is Parent B the representative payee?")]
        public bool IsParentBRepresentativePayee { get; set; }

        [Display(Name = "What is the monthly amount each parent is ordered to pay for spousal maintenance?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentASpousalMaintenance { get; set; }

        [Display(Name = "What is the monthly amount each parent is ordered to pay for spousal maintenance? ")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentBSpousalMaintenance { get; set; }

        [Display(Name = "What is the total amount each parent is ordered to pay for monthly child support for nonjoint child(ren)?")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentANonJointChildSupport { get; set; }

        [Display(Name = "What is the total amount each parent is ordered to pay for monthly child support for nonjoint child(ren)? ")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Must be number greater than or equal to 0.")]
        public decimal? ParentBNonJointChildSupport { get; set; }

        [Display(Name = "What is the number of nonjoint child(ren) living in the home?")]
        [Range(typeof(int), "0", "50", ErrorMessage = "Must be number less than 50.")]
        public int ParentANonJointChildren { get; set; }

        [Display(Name = "What is the number of nonjoint child(ren) living in the home?")]
        [Range(typeof(int), "0", "50", ErrorMessage = "Must be number less than 50.")]
        public int ParentBNonJointChildren { get; set; }


      public IEnumerator GetEnumerator()
      {
        throw new System.NotImplementedException();
      }
    }
}