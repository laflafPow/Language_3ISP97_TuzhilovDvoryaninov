//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Language_3ISP97_TuzhilovDvoryaninov.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientServiceDocument
    {
        public int ID { get; set; }
        public int IDClientService { get; set; }
        public string Document { get; set; }
    
        public virtual ClientService ClientService { get; set; }
    }
}
