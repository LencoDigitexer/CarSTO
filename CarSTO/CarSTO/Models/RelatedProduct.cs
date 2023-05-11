using System;
using System.Collections.Generic;

namespace CarSTO.Models;

public partial class RelatedProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int RelatedProdutId { get; set; }

    public virtual Cars Car { get; set; } = null!;

    public virtual Cars RelatedProdut { get; set; } = null!;
}
