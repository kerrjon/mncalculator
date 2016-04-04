using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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
      var combinedSupportObligation = 2727;
      var parentBOvernights = Math.Round(365 - input.ParentAOvernights, 1);

      var model = new CalculatorResult
      {
        ParentA = new Parent
        {
          Name = input.ParentA,
          GrossIncome = (double)Math.Round(input.ParentAGrossIncome, 0),
          PercentageOfParentingTime =
                  Math.Round(input.ParentAOvernights / (input.ParentAOvernights + parentBOvernights), 3) * 100,
          Overnights = Math.Round(input.ParentAOvernights, 1),
          PicsAmount = (double)input.ParentAGrossIncome, //todo: subtract other child
          PicsPercentage =
                  (double)
                      (Math.Round(input.ParentAGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome),
                          3) * 100),
          ProRataBasicSupportObligation =
                  (double)
                      Math.Round(
                          input.ParentAGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome) *
                          combinedSupportObligation, 2)
        },
        ParentB = new Parent
        {
          Name = input.ParentB,
          GrossIncome = (double)Math.Round(input.ParentBGrossIncome, 0),
          PercentageOfParentingTime =
                  Math.Round(parentBOvernights / (input.ParentAOvernights + parentBOvernights), 3) * 100,
          Overnights = parentBOvernights,
          PicsAmount = (double)input.ParentBGrossIncome, //todo: subtract other child
          PicsPercentage =
                  (double)
                      (Math.Round(input.ParentBGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome),
                          3) * 100),
          ProRataBasicSupportObligation =
                  (double)
                      Math.Round(
                          input.ParentBGrossIncome / (input.ParentAGrossIncome + input.ParentBGrossIncome) *
                          combinedSupportObligation, 2)
        },
        Combined = new Parent
        {
          Name = string.Empty,
          GrossIncome = (double)Math.Round(input.ParentAGrossIncome + input.ParentBGrossIncome, 0),
          PercentageOfParentingTime = 100,
          Overnights = input.ParentAOvernights + parentBOvernights,
          PicsAmount = (double)(input.ParentAGrossIncome + input.ParentBGrossIncome),
          //todo: subtract other child
          PicsPercentage = 100,
        },
        
        NumberOfChildren = input.NumberOfChildren,
        CourtFileNumber = input.CourtFileNumber,
        CaseNumber = input.CaseNumber,

      };


      var serializer = new XmlSerializer(typeof(Obligation));
      using (var reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/SupportObligation.xml")))
      {
        
        var obligationData = (Obligation)serializer.Deserialize(reader);
        var incomeBracket = obligationData.Incomes.Where(i => model.Combined.PicsAmount >= i.Min && model.Combined.PicsAmount <= i.Max).Select(x => x.Amounts);
       

      }

      model.CombinedBasicSupportObligation = combinedSupportObligation; //ToDo: get from xml

      var parentingTimeAdjusment = ((Math.Pow(parentBOvernights, 3.00) * model.ParentA.ProRataBasicSupportObligation) -
                                    (Math.Pow(input.ParentAOvernights, 3.00) * model.ParentB.ProRataBasicSupportObligation)) /
                                   (Math.Pow(parentBOvernights, 3.00) + (Math.Pow(input.ParentAOvernights, 3.00)));


      if (parentingTimeAdjusment < 0) //Parent B is obligor (it is not A as described in bill?)
      {
        model.ParentA.BasicSupportObligationAfterAdjustment = 0;
        model.ParentB.BasicSupportObligationAfterAdjustment = Math.Round(parentingTimeAdjusment * -1, 2);
      }
      else
      {
        model.ParentA.BasicSupportObligationAfterAdjustment = Math.Round(parentingTimeAdjusment, 2);
        model.ParentB.BasicSupportObligationAfterAdjustment = 0;
      }

      return model;
    }
  }

  public class Parent
  {
    public string Name { get; set; }
    public double Overnights { get; set; }

    [Display(Name = "Gross Income")]
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