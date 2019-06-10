using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ProjectFodMap.Data.Models
{
    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 2, ErrorMessage = "* Please Enter a Valid Name. 2 - 40 Letters")]
        public string MealName { get; set; }

        [Required]
        [StringLength(maximumLength: 120, MinimumLength = 2, ErrorMessage = "* Please Enter a Ingredient. 2 - 40 Letters")]
        public string MealIngredients { get; set; }

        public string MealInstructions { get; set; }

        public string MealNotes { get; set; }

        public string MealRisks { get; set; }
    }
}