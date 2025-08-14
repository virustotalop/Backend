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
public class Account
{
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [Required]
    [JsonPropertyName("number")]
    public required string Number { get; set; }

    [Required]
    [JsonPropertyName("current_balance")]
    public required decimal CurrentBalance { get; set; }

    [Required]
    [JsonPropertyName("overdraft_limit")]
    public required int OverdraftLimit { get; set; }

}