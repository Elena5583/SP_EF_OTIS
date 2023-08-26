using System;
using System.Collections.Generic;

namespace SP_EF_OTIS.Entities;

public partial class Client
{
    public Guid Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Inn { get; set; }

    public int? Age { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Bankaccount> Bankaccounts { get; set; } = new List<Bankaccount>();
}
