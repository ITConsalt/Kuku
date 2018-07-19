﻿using Kuku.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.ViewModels
{
    public class TypeOfDishesListViewModel
    {
        public IEnumerable<TypeOfDish> TypeOfDishes { get; set; }
        //public SelectList ProductTypes { get; set; }
        public string Name { get; set; }
        //public int Id { get; set; }
        public Recipe Recipe { get; set; }
    }
}
