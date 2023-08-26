using System;
using System.Collections.Generic;

namespace SP_EF_OTIS.Entities;

public partial class Bankaccount
{
    public Guid Id { get; set; }

    public Guid? Clientid { get; set; }

    public Guid? Currencyid { get; set; }

    public decimal? Bankaccountturnover { get; set; }

    public string? Numberaccount { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Currency? Currency { get; set; }
}
