using System.ComponentModel.DataAnnotations;

namespace MnCalculator.Models
{
    public class CalculatorResult
    {
        public Parent ParentA { get; set; }
        public Parent ParentB { get; set; }
        public Parent Combined { get; set; }
        [Display(Name = "Combined Basic Support Obligation")]
        public decimal CombinedBasicSupportObligation { get; set; }
        [Display(Name = "Number of Children")]
        public int NumberOfChildren { get; set; }
        [Display(Name = "IV-D Case Number")]
        public string CaseNumber { get; set; }
        [Display(Name = "Court File Number")]
        public string CourtFileNumber { get; set; }

        public static CalculatorResult Create(ChildSupportForm input)
        {
            var combinedSupportObligation = 2777;
            var parentBOvernights = 365 - input.ParentAOvernights;
            var model = new CalculatorResult
            {
                ParentA = new Parent
                {
                    Name = input.ParentA,
                    GrossIncome = decimal.Round(input.ParentAGrossIncome, 0),
                    PercentageOfParentingTime = decimal.Round(input.ParentAOvernights/(input.ParentAOvernights + parentBOvernights) *100, 1),
                    Overnights = decimal.Round(input.ParentAOvernights, 1),
                    PicsAmount = input.ParentAGrossIncome, //todo: subtract other child
                    PicsPercentage = decimal.Round(input.ParentAGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome),3) * 100,
                    ProRataBasicSupportObligation = decimal.Round(input.ParentAGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome) * combinedSupportObligation,2)
                },
                ParentB = new Parent
                {
                    Name = input.ParentB,
                    GrossIncome = decimal.Round(input.ParentBGrossIncome, 0),
                    PercentageOfParentingTime = decimal.Round(parentBOvernights / (input.ParentAOvernights + parentBOvernights) *100, 1),
                    Overnights = decimal.Round(input.ParentAOvernights, 1),
                    PicsAmount = input.ParentBGrossIncome, //todo: subtract other child
                    PicsPercentage = decimal.Round(input.ParentBGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome),3) * 100,
                    ProRataBasicSupportObligation = decimal.Round(input.ParentBGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome) * combinedSupportObligation, 2)
                },
                Combined = new Parent
                {
                    Name = string.Empty,
                    GrossIncome = decimal.Round(input.ParentAGrossIncome + input.ParentBGrossIncome, 0),
                    PercentageOfParentingTime = 100,
                    Overnights = input.ParentAOvernights + parentBOvernights,
                    PicsAmount = input.ParentAGrossIncome + input.ParentBGrossIncome, //todo: subtract other child
                    PicsPercentage = 100,
                },
                CombinedBasicSupportObligation = combinedSupportObligation,  //ToDo: get from xml
                NumberOfChildren = input.NumberOfChildren,
                CourtFileNumber = input.CourtFileNumber,
                CaseNumber = input.CaseNumber


            };

           


            return model;
        }
    }

    public class Parent
    {
        public string Name { get; set; }
        public decimal Overnights { get; set; }

        [Display(Name="Gross Income")]
        public decimal GrossIncome { get; set; }

        [Display(Name = "Percentage of Parenting Time")]
        public decimal PercentageOfParentingTime { get; set; }

        [Display(Name = "Percentage Share of Combined PICS")]
        public decimal PicsPercentage { get; set; }

        [Display(Name = "Parental Income for Determining Child Support")]
        public decimal PicsAmount { get; set; }

        [Display(Name = "Pro Rata Basic Support Obligation")]
        public decimal ProRataBasicSupportObligation { get; set; }

        [Display(Name = "Adjustment for Parenting Time")]
        public decimal AdjustmentForParentingTime { get; set; }

        [Display(Name = "Obligation After Parenting Time Adjustment")]
        public decimal BasicSupportObligationAfterAdjustment { get; set; }
    }
}