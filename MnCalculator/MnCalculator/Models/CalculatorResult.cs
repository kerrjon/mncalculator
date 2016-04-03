using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MnCalculator.Models
{
    public class CalculatorResult
    {
        public string Result { get; set; }

        public static CalculatorResult Create(ChildSupportForm input)
        {
            var model = new CalculatorResult() {Result = "test" };
            return model;
        }
    }
}