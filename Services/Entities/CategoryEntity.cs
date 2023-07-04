﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryEntity
    {
        [Column("CategoryId")]
        public int Id { get; set; }
        [Column("CategoryName")]
        public string Name { get; set; }
    }
}
