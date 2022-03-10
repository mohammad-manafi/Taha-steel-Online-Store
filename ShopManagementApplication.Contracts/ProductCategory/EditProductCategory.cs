using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using System.ComponentModel.DataAnnotations;


namespace ShopManagement.Application.Contracts.ProductCategory
{
    public class EditProductCategory : CreateProductCategory
    {
        public long Id { get; set; }
    }
}
