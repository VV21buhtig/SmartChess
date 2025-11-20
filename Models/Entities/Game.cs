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
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string InitialPosition { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }

        [MaxLength(20)]
        public string Result { get; set; } = "InProgress";

        public int MoveCount { get; set; } = 0;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Move> Moves { get; set; } = new List<Move>();
    }
}