using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class TeamMember:IEntity
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string UserEmail{ get; set; }
        public string Role{ get; set; }
    }
}
