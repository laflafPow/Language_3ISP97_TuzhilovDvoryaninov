using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_3ISP97_TuzhilovDvoryaninov.EF
{
    public partial class Client
    {
        public int CountVisit
        {
            get
            {
                return ClientService.Count();
            }
        }

        public DateTime? LastVisit
        {
            get
            {
                return ClientService.LastOrDefault()?.DateStart;
            }
        }

        public List<Tag> Tags
        {
            get
            {
                return Tag.ToList();
            }
        }

        public string FIO { get => $"{LastName} {FirstName} {Patronymic}".ToLower(); }
    }
}
