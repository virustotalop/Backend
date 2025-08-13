using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[PrimaryKey(nameof(Id))]
public class Transaction
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DebitOrCredit DebitCredit { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int AccountId { get; set; }

    public enum DebitOrCredit
    {
        Debit,
        Credit
    }

}