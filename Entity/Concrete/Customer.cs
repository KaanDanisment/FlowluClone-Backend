
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Customer:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId {  get; set; }
        public string? Email { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
    }
}
