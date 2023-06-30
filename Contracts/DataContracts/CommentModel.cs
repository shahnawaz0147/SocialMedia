using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DataContracts
{
    [Table("Comment")]
    public class CommentModel
    {
        private int m_ID;
        private string? m_Comment;
        private int m_PostID;

        [Key]
        public int ID { get => m_ID; set => m_ID = value; }
        public string? Comment { get => m_Comment; set => m_Comment = value; }
        public int PostID { get => m_PostID; set => m_PostID = value; }
    }
}
