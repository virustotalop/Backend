using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[PrimaryKey(nameof(Id))]
public class Transaction
{
    public int Id { get; set; }

    [Required]
    [JsonPropertyName ("description")]
    public required string Description { get; set; }

    [Required]
    [JsonPropertyName("debit_credit")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required DebitOrCredit DebitCredit { get; set; }

    [Required]
    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [Required]
    [JsonPropertyName("account_id")]
    public required int AccountId { get; set; }

    public enum DebitOrCredit
    {
        Debit,
        Credit
    }

}