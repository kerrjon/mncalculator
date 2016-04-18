using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

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
      var parentBOvernights = Math.Round(365 - input.ParentAOvernights, 1);

      if (input.ParentASocialSecurityBenefits == null){input.ParentASocialSecurityBenefits = 0;}
      if (input.ParentBSocialSecurityBenefits == null){input.ParentBSocialSecurityBenefits = 0;}
      if (input.ParentASpousalMaintenance == null) { input.ParentASpousalMaintenance = 0; }
      if (input.ParentBSpousalMaintenance == null) { input.ParentBSpousalMaintenance = 0; }
      if (input.ParentAPotentialIncome == null) { input.ParentAPotentialIncome = 0; }
      if (input.ParentBPotentialIncome == null) { input.ParentBPotentialIncome = 0; }
      if (input.ParentANonJointChildSupport == null) { input.ParentANonJointChildSupport = 0; }
      if (input.ParentBNonJointChildSupport == null) { input.ParentBNonJointChildSupport = 0; }


      var parentAGrossIncome = input.ParentAMonthlyIncomeReceived + input.ParentASocialSecurityBenefits +
                               input.ParentAPotentialIncome - input.ParentASpousalMaintenance -
                               input.ParentANonJointChildSupport;
      var parentBGrossIncome = input.ParentBMonthlyIncomeReceived + input.ParentBSocialSecurityBenefits +
                               input.ParentBPotentialIncome - input.ParentBSpousalMaintenance -
                               input.ParentBNonJointChildSupport;

      var model = new CalculatorResult
      {
        ParentA = new Parent
        {
          Name = input.ParentA,
          MonthlyIncomeReceived = (double)input.ParentAMonthlyIncomeReceived,
          SocialSecurityBenefits = (double)input.ParentASocialSecurityBenefits,
          PotenialIncome = (double)input.ParentAPotentialIncome,
          SpousalMaintenance = (double)input.ParentASpousalMaintenance,
          NonJointChildSupport = (double)input.ParentANonJointChildSupport,
          GrossIncome = Math.Round((double) parentAGrossIncome, 0),
          NumberOfNonJointChildren = input.ParentANonJointChildren,
          PercentageOfParentingTime =
                        Math.Round(input.ParentAOvernights / (input.ParentAOvernights + parentBOvernights), 3) * 100,
          Overnights = Math.Round(input.ParentAOvernights, 1),
          PicsAmount = (double) parentAGrossIncome, 
          PicsPercentage =
                        (double)
                            (Math.Round(input.ParentAMonthlyIncomeReceived / (input.ParentAMonthlyIncomeReceived + input.ParentBMonthlyIncomeReceived),
                                3) * 100)
        },
        ParentB = new Parent
        {
          Name = input.ParentB,
          MonthlyIncomeReceived = (double)input.ParentBMonthlyIncomeReceived,
          SocialSecurityBenefits = (double)input.ParentBSocialSecurityBenefits,
          PotenialIncome = (double)input.ParentBPotentialIncome,
          SpousalMaintenance = (double)input.ParentBSpousalMaintenance,
          NonJointChildSupport = (double)input.ParentBNonJointChildSupport,
          GrossIncome = Math.Round((double) parentBGrossIncome, 0),
          NumberOfNonJointChildren = input.ParentBNonJointChildren,
          PercentageOfParentingTime =
                        Math.Round(parentBOvernights / (input.ParentAOvernights + parentBOvernights), 3) * 100,
          Overnights = parentBOvernights,
          PicsAmount = (double)parentBGrossIncome, 
          PicsPercentage =
                        (double)
                            (Math.Round(input.ParentBMonthlyIncomeReceived / (input.ParentAMonthlyIncomeReceived + input.ParentBMonthlyIncomeReceived),
                                3) * 100)
        },
        Combined = new Parent
        {
          Name = string.Empty,
          GrossIncome = (double)Math.Round(input.ParentAMonthlyIncomeReceived + input.ParentBMonthlyIncomeReceived, 0),
          PercentageOfParentingTime = 100,
          Overnights = input.ParentAOvernights + parentBOvernights,
          PicsPercentage = 100
        },
        NumberOfChildren = input.NumberOfChildren,
        CourtFileNumber = input.CourtFileNumber,
        CaseNumber = input.CaseNumber
      };

      //lookup the obligation amount from XML and calculate PICS
      var serializer = new XmlSerializer(typeof(Obligation));
      using (var reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/SupportObligation.xml")))
      {
        var obligationData = (Obligation)serializer.Deserialize(reader);
        model.ParentA.PicsAmount = (double)parentAGrossIncome;
        model.ParentB.PicsAmount = (double)parentBGrossIncome;

        if (model.ParentA.NumberOfNonJointChildren > 0) // update the discount, if applicable
        {
          model.ParentA.DeductionForNonJointChildren = obligationData.Incomes.Where(i => model.ParentA.GrossIncome >= i.Min && model.ParentA.GrossIncome <= i.Max).SelectMany(i => i.Amounts).First(x => x.Children == model.ParentA.NumberOfNonJointChildren).Value / 2;
          model.ParentA.PicsAmount = model.ParentA.PicsAmount - model.ParentA.DeductionForNonJointChildren;
        }

        if (model.ParentB.NumberOfNonJointChildren > 0) // update the discount, if applicable
        {
          model.ParentB.DeductionForNonJointChildren = obligationData.Incomes.Where(i => model.ParentB.GrossIncome >= i.Min && model.ParentB.GrossIncome <= i.Max).SelectMany(i => i.Amounts).First(x => x.Children == model.ParentB.NumberOfNonJointChildren).Value / 2;
          model.ParentB.PicsAmount = model.ParentB.PicsAmount - model.ParentB.DeductionForNonJointChildren;
        }

        model.Combined.PicsAmount = model.ParentA.PicsAmount + model.ParentB.PicsAmount;
        model.ParentA.PicsPercentage = Math.Round(model.ParentA.PicsAmount / model.Combined.PicsAmount, 3) * 100;
        model.ParentB.PicsPercentage = Math.Round(model.ParentB.PicsAmount / model.Combined.PicsAmount, 3) * 100;
      
        model.CombinedBasicSupportObligation = obligationData.Incomes.Where(i => model.Combined.PicsAmount >= i.Min && model.Combined.PicsAmount <= i.Max).SelectMany(i => i.Amounts).First(x => x.Children == input.NumberOfChildren).Value;
      }

      model.ParentA.ProRataBasicSupportObligation = Math.Round(Math.Round(model.ParentA.PicsPercentage / 100, 2) * (double) model.CombinedBasicSupportObligation, 0);
      model.ParentB.ProRataBasicSupportObligation = Math.Round(Math.Round(model.ParentB.PicsPercentage / 100, 2) * (double) model.CombinedBasicSupportObligation, 0);

      //calculate the parenting time adjustment
      var parentingTimeAdjusment = ((Math.Pow(parentBOvernights, 3.00) * model.ParentA.ProRataBasicSupportObligation) -
                              (Math.Pow(input.ParentAOvernights, 3.00) * model.ParentB.ProRataBasicSupportObligation)) /
                             (Math.Pow(parentBOvernights, 3.00) + (Math.Pow(input.ParentAOvernights, 3.00)));

      if (parentingTimeAdjusment < 0) //Parent B is obligor (it is not A as described in bill?)
      {
        model.ParentA.BasicSupportObligationAfterAdjustment = 0;
        model.ParentB.BasicSupportObligationAfterAdjustment = Math.Round(parentingTimeAdjusment * -1, 0);
      }
      else
      {
        model.ParentA.BasicSupportObligationAfterAdjustment = Math.Round(parentingTimeAdjusment, 0);
        model.ParentB.BasicSupportObligationAfterAdjustment = 0;
      }

      return model;
    }
  }

  public class Parent
  {
    public string Name { get; set; }
    public double Overnights { get; set; }

    [Display(Name = "1a. Monthly Income Received")]
    public double MonthlyIncomeReceived { get; set; }

    [Display(Name = "1b. Child(ren)'s Social Security/Veterans' Benefits Derived From a Parent's Eligibility")]
    public double SocialSecurityBenefits { get; set; }

    [Display(Name = "1c. Potential Income")]
    public double PotenialIncome { get; set; }

    [Display(Name = "1d. Spousal Maintenance Orders Obligated to be Paid")]
    public double SpousalMaintenance { get; set; }

    [Display(Name = "1e. Child Support Order(s) Obligated to be Paid for Nonjoint Child(ren)")]
    public double NonJointChildSupport { get; set; }

    [Display(Name = "1f. Monthly Gross Income (1a+1b+1c-1d-1e)")]
    public double GrossIncome { get; set; }

    [Display(Name = "Percentage of Parenting Time")]
    public double PercentageOfParentingTime { get; set; }

    [Display(Name = "Percentage Share of Combined PICS")]
    public double PicsPercentage { get; set; }

    [Display(Name = "Parental Income for Determining Child Support")]
    public double PicsAmount { get; set; }

    [Display(Name = "Pro Rata Basic Support Obligation")]
    public double ProRataBasicSupportObligation { get; set; }

    [Display(Name = "Adjustment for Parenting Time")]
    public double AdjustmentForParentingTime { get; set; }

    [Display(Name = "Obligation After Parenting Time Adjustment")]
    public double BasicSupportObligationAfterAdjustment { get; set; }

    [Display(Name = "2a.Number of Nonjoint Child(ren) in the Home(Maximum number allowed is 2)")]
    public int NumberOfNonJointChildren { get; set; }

    [Display(Name = "2b. Deduction for Nonjoint Child(ren) in the Home")]
    public double DeductionForNonJointChildren { get; set; }
  }

  public class Income
  {
    [XmlAttribute("min")]
    public int Min { get; set; }

    [XmlAttribute("max")]
    public int Max { get; set; }

    [XmlArray("amounts")]
    [XmlArrayItem("amount")]
    public List<Amount> Amounts { get; set; }
  }

  public class Amount
  {
    [XmlAttribute("children")]
    public int Children { get; set; }

    [XmlAttribute("value")]
    public int Value { get; set; }
  }

  [XmlRoot("obligation")]
  public class Obligation
  {
    [XmlArray("incomes")]
    [XmlArrayItem("income")]
    public List<Income> Incomes { get; set; }
  }
}