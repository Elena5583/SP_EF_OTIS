using System;
using System.Collections.Generic;

namespace SP_EF_OTIS.Entities;

public partial class Currency
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Shortname { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Bankaccount> Bankaccounts { get; set; } = new List<Bankaccount>();
}
