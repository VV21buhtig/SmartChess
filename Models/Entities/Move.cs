using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartChess.Models;

namespace SmartChess.Models.Entities
{
    [Table("Moves")]
    public class Move
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GameId { get; set; }

        public int MoveNumber { get; set; }

        [Required]
        [MaxLength(2)]
        public string FromPosition { get; set; } = null!;

        [Required]
        [MaxLength(2)]
        public string ToPosition { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string PieceType { get; set; } = null!;

        [Required]
        [MaxLength(5)]
        public string Color { get; set; } = null!;

        [MaxLength(10)]
        public string? Promotion { get; set; }

        public bool IsCapture { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCheckmate { get; set; }
        public string? CapturedPiece { get; set; }

        [ForeignKey("GameId")]
        public virtual Game Game { get; set; } = null!;
    }
}