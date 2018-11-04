using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFodMap.Data.ViewModels
{
    public class MealDataVM
    {
        public int MealID { get; set; }

        public string MealName { get; set; }

        public string MealIngredients { get; set; }

        public string MealInstructions { get; set; }

        public string MealNotes { get; set; }

        public string MealRisks { get; set; }
    }
}
