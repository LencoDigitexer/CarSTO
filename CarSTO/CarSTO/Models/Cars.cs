using CarSTO.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace CarSTO.Models;

public partial class Cars : ViewModel
{
    public int? Id { get; set; }
    public string? Gosnomer { get; set; } 
    public string? Vladelec { get; set; } = null!;
    public string? Marka { get; set; } = null!;
    public string? Osmotr { get; set; } = null!;
    public string? Vidacha { get; set; } = null!;
    public string? Cost { get; set; } = null!;
    public string? Category { get; set; } = null!;
    public string? Vipusk { get; set; } = null!;
    public string? VIN { get; set; } = null!;
    public string? PTS { get; set; } = null!;
    public string? Tormoz { get; set; } = null!;
    public string? Rul { get; set; } = null!;
    public string? Shini { get; set; } = null!;
    public string? Diski { get; set; } = null!;
    public string? Dvigatel { get; set; } = null!;
    public string? Prochie { get; set; } = null!;




    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual ICollection<RelatedProduct> RelatedProductProducts { get; } = new List<RelatedProduct>();

    public virtual ICollection<RelatedProduct> RelatedProductRelatedProduts { get; } = new List<RelatedProduct>();
}
