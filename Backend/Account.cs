using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[PrimaryKey(nameof(Id))]
public class Account
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Number { get; set; }

    [Required]
    public decimal CurrentBalance { get; set; }

    [Required]
    public int OverdraftLimit { get; set; }

}